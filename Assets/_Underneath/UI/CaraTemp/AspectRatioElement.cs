using UnityEngine;
using UnityEngine.UIElements;

public class AspectRatioElement : VisualElement, System.IDisposable
{
    public enum FitMode { ExplicitSize, LockWidth, LockHeight }

    public float AspectRatio { get => _aspectRatio; set { _aspectRatio = Mathf.Max(0.0001f, value); ApplySizing(); } }
    public FitMode Mode { get => _mode; set { _mode = value; ApplySizing(); } }
    public float MinSize { get => _minSize; set { _minSize = Mathf.Max(0f, value); ApplySizing(); } }
    public float MaxSize { get => _maxSize; set { _maxSize = Mathf.Max(0f, value); ApplySizing(); } }

    // ---- UXML support (shows in UI Builder) ----
    public new class UxmlFactory : UxmlFactory<AspectRatioElement, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        readonly UxmlFloatAttributeDescription _aspect = new() { name = "aspect-ratio", defaultValue = 1.0f };
        readonly UxmlEnumAttributeDescription<FitMode> _mode = new() { name = "fit-mode", defaultValue = FitMode.LockWidth };
        readonly UxmlFloatAttributeDescription _min = new() { name = "min-size", defaultValue = 0f };
        readonly UxmlFloatAttributeDescription _max = new() { name = "max-size", defaultValue = 1000f };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var el = (AspectRatioElement)ve;
            el._aspectRatio = Mathf.Max(0.0001f, _aspect.GetValueFromBag(bag, cc));
            el._mode = _mode.GetValueFromBag(bag, cc);
            el._minSize = Mathf.Max(0f, _min.GetValueFromBag(bag, cc));
            el._maxSize = Mathf.Max(0f, _max.GetValueFromBag(bag, cc));
            el.AddToClassList("aspect-ratio-element");
            el.ApplySizing();
        }
    }

    static readonly CustomStyleProperty<float> kStyleAspect = new("--aspect-ratio");
    static readonly CustomStyleProperty<string> kStyleMode = new("--fit-mode");
    static readonly CustomStyleProperty<float> kStyleMin = new("--min-size");
    static readonly CustomStyleProperty<float> kStyleMax = new("--max-size");

    float _aspectRatio = 1.0f;
    FitMode _mode = FitMode.LockWidth;
    float _minSize = 0f;
    float _maxSize = 1000f;
    
    private bool _disposed = false;

    public AspectRatioElement()
    {
        // react to geometry and style changes
        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        style.overflow = Overflow.Visible;
    }
    
    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        if (!_disposed) ApplySizing();
    }

    public void Dispose()
    {
        if (_disposed) return;
        
        UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        UnregisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
        
        _disposed = true;
    }

    void OnCustomStyleResolved(CustomStyleResolvedEvent e)
    {
        if (_disposed) return;
        
        if (e.customStyle.TryGetValue(kStyleAspect, out var a)) _aspectRatio = Mathf.Max(0.0001f, a);
        if (e.customStyle.TryGetValue(kStyleMin, out var mn)) _minSize = Mathf.Max(0f, mn);
        if (e.customStyle.TryGetValue(kStyleMax, out var mx)) _maxSize = Mathf.Max(0f, mx);

        if (e.customStyle.TryGetValue(kStyleMode, out var mStr) && TryParseEnum(mStr, out FitMode m)) _mode = m;

        ApplySizing();
    }

    static bool TryParseEnum<T>(string s, out T value) where T : struct
    {
        if (string.IsNullOrEmpty(s)) { value = default; return false; }
        return System.Enum.TryParse<T>(s.Trim(), true, out value);
    }

    float GetParentWidth()
    {
        if (parent == null) return 0f;
        var rs = parent.resolvedStyle;
        var w = !float.IsNaN(rs.width) && rs.width > 0 ? rs.width : parent.layout.width;
        return w > 0 ? w : 0f;
    }

    float GetParentHeight()
    {
        if (parent == null) return 0f;
        var rs = parent.resolvedStyle;
        var h = !float.IsNaN(rs.height) && rs.height > 0 ? rs.height : parent.layout.height;
        return h > 0 ? h : 0f;
    }

    float PercentToPixels(float percent, float reference)
    {
        return Mathf.Max(0f, reference) * Mathf.Clamp01(percent / 100f);
    }

    void ApplySizing()
    {
        if (_disposed || panel == null) return;
        
        float w, h;

        switch (_mode)
        {
            case FitMode.LockWidth:
            {
                // Use the element's own width, fallback to parent if not set
                w = resolvedStyle.width;
                if (float.IsNaN(w) || w <= 0)
                    w = layout.width > 0 ? layout.width : GetParentWidth();
                if (w <= 0) w = 100f; // final fallback
                
                h = w / _aspectRatio;
                break;
            }

            case FitMode.LockHeight:
            {
                // Use the element's own height, fallback to parent if not set
                h = resolvedStyle.height;
                if (float.IsNaN(h) || h <= 0)
                    h = layout.height > 0 ? layout.height : GetParentHeight();
                if (h <= 0) h = 100f; // final fallback
                
                w = h * _aspectRatio;
                break;
            }

            case FitMode.ExplicitSize:
            default:
            {
                // For explicit size, you could use either width or height as the base
                // depending on aspect ratio, or always use one dimension
                if (_aspectRatio >= 1f)
                {
                    w = resolvedStyle.width;
                    if (float.IsNaN(w) || w <= 0) w = 100f;
                    h = w / _aspectRatio;
                }
                else
                {
                    h = resolvedStyle.height;
                    if (float.IsNaN(h) || h <= 0) h = 100f;
                    w = h * _aspectRatio;
                }
                break;
            }
        }

        // Apply constraints
        w = Mathf.Clamp(w, _minSize, _maxSize);
        h = Mathf.Clamp(h, _minSize, _maxSize);

        // Only set the dimension we're NOT locking
        switch (_mode)
        {
            case FitMode.LockWidth:
                style.height = h;
                break;
            case FitMode.LockHeight:
                style.width = w;
                break;
            case FitMode.ExplicitSize:
                style.width = w;
                style.height = h;
                break;
        }
    }
}
