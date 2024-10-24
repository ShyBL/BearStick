using UnityEngine;

public class DrawLineRenderer : OurMonoBehaviour
{
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _lineRenderer= GetComponent<LineRenderer>();
        if (_lineRenderer != null)
        {
            _lineRenderer.useWorldSpace = false; // So that the line renderer follows the sprite transformations
        }
        
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        if (_polygonCollider2D != null)
        {
            _polygonCollider2D.isTrigger = true; // So that the player cannot collide with it.
        }
         
    }
    
    private void Start()
    {
        DrawLine();
    }
    
    public void BeginFocus()
    {
        if(_lineRenderer != null)
        {
            _lineRenderer.enabled = true;
        }
    }

    public void EndFocus()
    {
        if(_lineRenderer != null )
        {
            _lineRenderer.enabled = false;
        }
    }

    public void DrawLine()
    {
        if (_lineRenderer != null && _polygonCollider2D != null)
        {
            // Set number of points in LineRenderer
            _lineRenderer.positionCount = _polygonCollider2D.points.Length + 1;

            // Set positions for line renderer points
            for (int i = 0; i < _polygonCollider2D.points.Length; i++)
            {
                _lineRenderer.SetPosition(i, _polygonCollider2D.points[i]);
            }

            // Close loop
            _lineRenderer.SetPosition(_polygonCollider2D.points.Length, _polygonCollider2D.points[0]);

            // Can make this width a variable, for testing purposes keeping this amount
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;

            _lineRenderer.enabled = false;
        }
    }
}