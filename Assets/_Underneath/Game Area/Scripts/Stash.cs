using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public enum StashType { Dumpster, Trashcan }
public class Stash : MonoBehaviour
{
    // Here is a list of pickups, serialized so level designers can add pickups to the list for the goal area.
    [SerializeField] 
    private List<Item> collectableList;
    [SerializeField] 
    private GameObject physicsCollectable;
    [SerializeField] 
    private GameObject textGameObject;
    [SerializeField]
    private GameObject artGameObject;
    [SerializeField]
    private Animator _animator;

    public StashType type;

    // This boolean determines if this goal area has been used or not.
    public bool opened = false;
    private bool inRange = false;
    

    // The player needs a "Player" tag. When the player collides with this object, a boolean is set to signal that the player is in range.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && opened == false)
        {
            inRange = true;
            textGameObject.SetActive(true);

            //Call Begin Focus
            BeginFocus();
        }
    }

    // When the player no longer is in contact with the object, the bool is unset.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            textGameObject.SetActive(false);

            //Call End Focus
            EndFocus();
        }
    }

    // Subscribes this object to the player's input action.
    private void Start()
    {
        Player.Instance.playerInput.onInteract += Interact;

        DrawLineRenderer();
    }

    [SerializeField] private Transform OutPoint;
    [SerializeField] private bool tutorialOpeningDoOnce;
    [SerializeField] private bool tutorialCarryingDoOnce;

    // Allows the input action to spawn collectibles from the list as long as the player is within range.
    private void Interact()
    {
        if (inRange && opened == false)
        {
            _animator.Play("Open");

            PlaySoundByType();

            Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting

            foreach (Item collectable in collectableList)
            {
                PhysicsCollectible newCollectable = Instantiate(physicsCollectable, OutPoint.position, quaternion.identity).GetComponent<PhysicsCollectible>();
                newCollectable.SetCollectable(collectable);
            }

            opened = true;
            textGameObject.SetActive(false);
            
            if (tutorialOpeningDoOnce)
            {
                TutorialManager.Instance.opening = true;
                tutorialOpeningDoOnce = false;
                
            }
        
            if (tutorialCarryingDoOnce)
            {
                TutorialManager.Instance.carrying = true;
                tutorialCarryingDoOnce = false;
            }
            
            Player.Instance.EnableMovement();
            EndFocus();
        }
    }

    private void BeginFocus()
    {
        //Set the LineRenderer to be enabled
        LineRenderer lineRenderer = artGameObject.GetComponent<LineRenderer>();
        if(lineRenderer != null && opened == false)
        {
            lineRenderer.enabled = true;
        }
    }

    private void EndFocus()
    {
        //Set the LineRenderer to be disabled
        LineRenderer lineRenderer = artGameObject.GetComponent<LineRenderer>();
        if(lineRenderer != null )
        {
            lineRenderer.enabled = false;
        }
    }

    private void DrawLineRenderer()
    {
        //Get a reference to the LineRenderer and PolygonCollider Components
        LineRenderer lineRenderer = artGameObject.GetComponent<LineRenderer>();
        PolygonCollider2D polygonCollider2D = artGameObject.GetComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = true; //So that the player cannot collide with it.

        //Check to make sure they are valid
        if (lineRenderer != null && polygonCollider2D != null)
        {
            //Set number of points in LineRenderer
            lineRenderer.positionCount = polygonCollider2D.points.Length + 1;

            //Set positions for line renderer points
            for (int i = 0; i < polygonCollider2D.points.Length; i++)
            {
                lineRenderer.SetPosition(i, polygonCollider2D.points[i]);
            }

            //Close loop
            lineRenderer.SetPosition(polygonCollider2D.points.Length, polygonCollider2D.points[0]);
            //So that the line renderer follows the sprite transformations 
            lineRenderer.useWorldSpace = false;
            //Can make this width a variable, for testing purposes keeping this amount
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            lineRenderer.enabled = false;
        }
    }

    private void PlaySoundByType()
    {
        switch (type)
        {
            case StashType.Dumpster:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Dumpster, transform.position);
                break;

            case StashType.Trashcan:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Trashcan, transform.position);
                break;
        }
    }
}