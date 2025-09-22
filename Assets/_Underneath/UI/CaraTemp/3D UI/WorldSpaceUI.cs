using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(UIDocument))]
public class WorldSpaceUI : MonoBehaviour
{
    [Header("Required References")]
    public Material UImaterial;
    
    [Header("Settings")]
    public Vector2 PanelResolution = new Vector2(800, 600);
    public float ScaleFactor = 1f;
    
    [Header("Input")]
    public LayerMask raycastLayerMask = -1;
    public Camera inputCamera;
    
    [Header("Debug")]
    public bool enableDebugLogs = false;
    public bool drawDebugRay = false;
    public bool showUIGizmos = true;
    
    [Header("Initialized")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private RenderTexture uiTexture;
    
    [Header("Assigned")]
    [SerializeField] private VisualTreeAsset visualTreeAsset;
    [SerializeField] private PanelSettings originalPanelSettings;

    private PanelSettings panelSettingsClone;

    private VisualElement lastHoveredElement;
    private bool isInitialized = false;

    private void OnEnable() => Initialize();

    private void Start()
    {
        if (Application.isPlaying)
            Initialize();
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            HandleInput();
        }
        
    }

    private void Initialize()
    {
        if (isInitialized) return;
        
        SetupComponents();
        CreateRenderTexture();
        SetupMeshComponents();
        ConfigurePanelSettings();
        SetupUIDocument();
        UpdateScale();
        
        if (inputCamera == null)
            inputCamera = Camera.main;
            
        isInitialized = true;
    }

    private void SetupComponents()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("WorldSpaceUI requires a UIDocument component");
            return;
        }
        
        visualTreeAsset = uiDocument.visualTreeAsset;
    }

    private void CreateRenderTexture()
    {
        if (uiTexture != null)
        {
            if (Application.isPlaying)
                Destroy(uiTexture);
            else
                DestroyImmediate(uiTexture);
        }
        
        uiTexture = new RenderTexture((int)PanelResolution.x, (int)PanelResolution.y, 16, RenderTextureFormat.ARGB32);
        uiTexture.antiAliasing = 1;
        uiTexture.enableRandomWrite = true;
        uiTexture.Create();
        
        ClearRenderTexture();
    }

    //clears to black with 0 alpha  
    private void ClearRenderTexture()
    {
        if (uiTexture != null && uiTexture.IsCreated())
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = uiTexture;
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            RenderTexture.active = currentRT;
        }
    }

    private void SetupMeshComponents()
    {
        meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        if (UImaterial != null)
        {
            meshRenderer.sharedMaterial = UImaterial;
            meshRenderer.sharedMaterial.SetTexture("_BaseMap", uiTexture);
        }
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;

        meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        meshFilter.sharedMesh = GenerateQuad();
        
        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        meshCollider.convex = false;
    }

    private void ConfigurePanelSettings()
    {
        // Clean up previous clone
        if (panelSettingsClone != null)
        {
            if (Application.isPlaying)
                Destroy(panelSettingsClone);
            else
                DestroyImmediate(panelSettingsClone);
        }

        // Always create a fresh clone from the original
        if (originalPanelSettings != null)
        {
            panelSettingsClone = Instantiate(originalPanelSettings);
            panelSettingsClone.targetTexture = uiTexture;
            uiDocument.panelSettings = panelSettingsClone;
        }
    }

    private void SetupUIDocument()
    {
        if (visualTreeAsset != null)
            uiDocument.visualTreeAsset = visualTreeAsset;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            EditorApplication.delayCall += () => { if (this != null) Initialize(); };


        //if the texture resultion nolong matches the panel settings
        if (uiTexture != null && panelSettingsClone != null)
        {
            if (uiTexture.width != (int)PanelResolution.x || uiTexture.height != (int)PanelResolution.y)
            {
                if (enableDebugLogs) Debug.Log("Panel resolution changed, recreating RenderTexture");
                CreateRenderTexture();
                if (panelSettingsClone != null)
                    panelSettingsClone.targetTexture = uiTexture;
            }
            UpdateScale();
        }
    }
#endif

    private void UpdateScale()
    {
        float aspectRatio = PanelResolution.x / PanelResolution.y;
        transform.localScale = new Vector3(aspectRatio * ScaleFactor, ScaleFactor, 1f);
    }

    private void OnDestroy() => CleanUp();
    private void OnDisable() 
    { 
        if (!Application.isPlaying) 
        {
            CleanUp();
            isInitialized = false;
        }
    }

    private void CleanUp()
    {
        if (uiTexture != null)
        {
            uiTexture.Release();
            if (Application.isPlaying)
                Destroy(uiTexture);
            else
                DestroyImmediate(uiTexture);
        }
    }

    private Mesh GenerateQuad()
    {
        var mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            new Vector3(-0.5f, -0.5f, 0), new Vector3(0.5f, -0.5f, 0),
            new Vector3(0.5f, 0.5f, 0), new Vector3(-0.5f, 0.5f, 0)
        };
        mesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
        mesh.uv = new Vector2[] {
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1)
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    private void HandleInput()
    {
        if (!inputCamera) 
        {
            if (enableDebugLogs) Debug.LogWarning("WorldSpaceUI: No input camera assigned");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (enableDebugLogs) Debug.Log("Mouse down detected");
            ProcessMouseInput(Input.mousePosition, true, false, false);
        }
        else if (Input.GetMouseButton(0))
            ProcessMouseInput(Input.mousePosition, false, true, false);
        else if (Input.GetMouseButtonUp(0))
        {
            if (enableDebugLogs) Debug.Log("Mouse up detected");
            ProcessMouseInput(Input.mousePosition, false, false, true);
        }
        else if (Time.frameCount % 2 == 0)
            ProcessMouseInput(Input.mousePosition, false, false, false);
    }

    private void ProcessMouseInput(Vector3 screenPosition, bool isDown, bool isHeld, bool isUp)
    {
        var ray = inputCamera.ScreenPointToRay(screenPosition);
        
        if (enableDebugLogs && (isDown || isUp))
            Debug.Log($"Casting ray from {screenPosition}");
        
        if (drawDebugRay)
        {
            var rayColor = isDown ? Color.green : (isUp ? Color.red : Color.yellow);
            Debug.DrawRay(ray.origin, ray.direction * 10f, rayColor, 0.1f);
        }
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastLayerMask))
        {
            if (enableDebugLogs && (isDown || isUp))
                Debug.Log($"Hit {hit.collider.name} on layer {hit.collider.gameObject.layer}");
            
            if (hit.collider == meshCollider)
            {
                if (enableDebugLogs && (isDown || isUp))
                    Debug.Log($"Hit mesh collider! Texture coord: {hit.textureCoord}");

                //draw the hit point on the mesh by drawing a box around it
                var boxSize = 0.1f;
                var boxCenter = hit.point;
                var color = new Color(1f, 0f, 0f, 0.25f);
                
                if (drawDebugRay)
                {   
                    Debug.DrawLine(boxCenter + new Vector3(-boxSize, -boxSize, 0), boxCenter + new Vector3(boxSize, -boxSize,0), color, .1f);
                    Debug.DrawLine(boxCenter + new Vector3(boxSize, -boxSize, 0), boxCenter + new Vector3(boxSize, boxSize, 0), color, .1f);
                    Debug.DrawLine(boxCenter + new Vector3(boxSize, boxSize, 0), boxCenter + new Vector3(-boxSize, boxSize, 0), color, .1f);
                    Debug.DrawLine(boxCenter + new Vector3(-boxSize, boxSize, 0), boxCenter + new Vector3(-boxSize, -boxSize, 0), color, .1f);
                }

                var uiPosition = ConvertToUIPosition(hit.textureCoord);
                SendUIEvent(uiPosition, isDown, isHeld, isUp);
            }
            else if (!isDown && !isUp && !isHeld)
                ClearHover();
        }
        else if (!isDown && !isUp && !isHeld)
            ClearHover();
    }

    private Vector2 ConvertToUIPosition(Vector2 textureCoord)
    {
        var uiPos = new Vector2(
            textureCoord.x * PanelResolution.x,
            (1f - textureCoord.y) * PanelResolution.y
        );
        
        uiPos.x = Mathf.Clamp(uiPos.x, 0, PanelResolution.x);
        uiPos.y = Mathf.Clamp(uiPos.y, 0, PanelResolution.y);
        
        if (enableDebugLogs)
            Debug.Log($"Texture coord {textureCoord} -> UI pos {uiPos}");
        
        return uiPos;
    }

    private void SendUIEvent(Vector2 uiPosition, bool isDown, bool isHeld, bool isUp)
    {
        if (uiDocument?.rootVisualElement == null) 
        {
            if (enableDebugLogs && (isDown || isUp))
                Debug.LogWarning("UIDocument or root element is null!");
            return;
        }

        var targetElement = uiDocument.rootVisualElement.panel?.Pick(uiPosition);
        
        if (enableDebugLogs && (isDown || isUp))
        {
            if (targetElement != null)
                Debug.Log($"Found element: {targetElement.GetType().Name} at {uiPosition}");
            else
                Debug.Log($"No element at {uiPosition}. Root bounds: {uiDocument.rootVisualElement.layout}");
        }
        
        if (!isDown && !isUp && !isHeld)
            UpdateHover(targetElement);
        
        if (targetElement != null)
        {
            if (isDown)
            {
                using var downEvt = MouseDownEvent.GetPooled();
                downEvt.target = targetElement;
                targetElement.SendEvent(downEvt);
                
                using var clickEvt = ClickEvent.GetPooled();
                clickEvt.target = targetElement;
                targetElement.SendEvent(clickEvt);
            }
            else if (isUp)
            {
                using var upEvt = MouseUpEvent.GetPooled();
                upEvt.target = targetElement;
                targetElement.SendEvent(upEvt);
            }
            else if (!isHeld)
            {
                using var moveEvt = MouseMoveEvent.GetPooled();
                moveEvt.target = targetElement;
                targetElement.SendEvent(moveEvt);
            }
        }
    }

    private void UpdateHover(VisualElement targetElement)
    {
        if (lastHoveredElement != null && lastHoveredElement != targetElement)
        {
            using var exitEvt = MouseLeaveEvent.GetPooled();
            exitEvt.target = lastHoveredElement;
            lastHoveredElement.SendEvent(exitEvt);
        }
        
        if (targetElement != null && targetElement != lastHoveredElement)
        {
            using var enterEvt = MouseEnterEvent.GetPooled();
            enterEvt.target = targetElement;
            targetElement.SendEvent(enterEvt);
        }
        
        lastHoveredElement = targetElement;
    }

    private void ClearHover()
    {
        if (lastHoveredElement != null)
        {
            using var exitEvt = MouseLeaveEvent.GetPooled();
            exitEvt.target = lastHoveredElement;
            lastHoveredElement.SendEvent(exitEvt);
            lastHoveredElement = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (showUIGizmos && meshCollider && inputCamera)
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, .01f));
        }
    }
}
