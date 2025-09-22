using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public enum HudLayout
{
    BorderHorizontal,
    Horizontal,
    Vertical,
}

public class HudControl : MonoBehaviour
{
    public UIDocument uiDocument;
    public VisualElement container;
    VisualElement root = null;

    public HudLayout layout;

    void OnEnable()
    {
        if (uiDocument == null) uiDocument = gameObject.GetComponent<UIDocument>();
        if (root == null) root = uiDocument.rootVisualElement;
        if (root == null) return;

        container = root.Q("Container");

        var a = container.Q(HudLayout.BorderHorizontal.ToString());
        var b = container.Q(HudLayout.Horizontal.ToString());
        var c = container.Q(HudLayout.Vertical.ToString());

        // Set the layout based on the selected layout
        switch (layout)
        {
            case HudLayout.BorderHorizontal:
                a.style.display = DisplayStyle.Flex;
                b.style.display = DisplayStyle.None;
                c.style.display = DisplayStyle.None;
                break;
            case HudLayout.Horizontal:
                a.style.display = DisplayStyle.None;
                b.style.display = DisplayStyle.Flex;
                c.style.display = DisplayStyle.None;
                break;
            case HudLayout.Vertical:
                a.style.display = DisplayStyle.None;
                b.style.display = DisplayStyle.None;
                c.style.display = DisplayStyle.Flex;
                break;
        }
    }

    void OnValidate()
    {
        OnEnable();
    }
}
