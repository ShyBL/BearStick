using UnityEngine;

public class InteractiveObject : OurMonoBehaviour
{
    // This boolean determines if this interaction can happen or not.
    protected bool InRange;
    protected Player Player;
    
    private LineRenderer _lineRenderer;
    private PolygonCollider2D _polygonCollider2D;
    private Animator _animator;
    
    [SerializeField] 
    protected string animName = "Idle";
    [SerializeField]
    private StashType interactType;
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        
        _lineRenderer= GetComponentInChildren<LineRenderer>();
        
        _polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
        _polygonCollider2D.isTrigger = true; // So that the player cannot collide with it.
        _lineRenderer.useWorldSpace = false; // So that the line renderer follows the sprite transformations 
    }
    
    protected void PlaySoundByType()
    {
        switch (interactType)
        {
            case StashType.Dumpster:
                GameManager.AudioManager.PlayOneShot(FMODEvents.Instance.Dumpster, transform.position);
                break;

            case StashType.Trashcan:
                GameManager.AudioManager.PlayOneShot(FMODEvents.Instance.Trashcan, transform.position);
                break;
            default:
                break;
        }
    }

    protected void PlayAnimation(string stateName)
    {
        if (_animator != null)
        {
            _animator.Play(stateName);
        }
    }
    
    protected void BeginFocus()
    {
        if(_lineRenderer != null)
        {
            _lineRenderer.enabled = true;
        }
    }

    protected void EndFocus()
    {
        if(_lineRenderer != null )
        {
            _lineRenderer.enabled = false;
        }
    }

    protected void DrawLineRenderer()
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