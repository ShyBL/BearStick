using System;
using System.Collections.Generic;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

public enum StashType { Dumpster, Trashcan }

public class StashData : ScriptableObject
{
    public List<Item> collectablesList;
    public Sprite stashSprite;
    public StashType type;
}

public class ExampleStash : MonoBehaviour
{
    [SerializeField] private StashData stashData;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private Transform popOutPoint;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Animator animator;
    
    private EventReference _stashSound;
    private List<GameObject> _collectablesList;
    private bool _inRange;

    private void Awake()
    {
        Initialize();
        Player.Instance.playerInput.onInteract += Interact;
    }

    private void Initialize()
    {
        // There are different sounds for each stash type
        // type can be used to run different logic for each stash
        
        switch (stashData.type)
        {
            case StashType.Dumpster:
                
                _stashSound = FMODEvents.instance.Dumpster;
                visual.sprite = stashData.stashSprite;
                break;
            
            case StashType.Trashcan:
                
                _stashSound = FMODEvents.instance.Trashcan;
                visual.sprite = stashData.stashSprite;
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        SetupCollectablesListFromData();
    }

    private void SetupCollectablesListFromData()
    {
        // Go over the list of items in the stash data
        // and initialize each collectible object with each data
        foreach (Item collectable in stashData.collectablesList)
        {
            GameObject obj = Instantiate(collectablePrefab, popOutPoint.position, quaternion.identity);
            PhysicsCollectible newCollectable = obj.GetComponent<PhysicsCollectible>();

            newCollectable.SetCollectable(collectable); // Initializing the data in the script
            
            // Storing it in a control list, making it invisible
            _collectablesList.Add(obj); 
            obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _inRange = false;
        }
    }

    private void Interact()
    {
        animator.Play("Open");
        AudioManager.instance.PlayOneShot(_stashSound,transform.position);
            
        Player.Instance.DisableMovement(); // Example of using Player capabilities, make sure the player is not moving while interacting

        if (_inRange)
        {
            foreach (GameObject collectable in _collectablesList)
            {
                collectable.SetActive(true);
            }
        }
        
        Player.Instance.EnableMovement();
    }
}

public class ExampleItemDetector : MonoBehaviour
{
    public GameObject detectedItem;
    public bool itemDetected;

    private void OnTriggerEnter(Collider other)
    {
        // replace PhysicsCollectible class with any other feature
        
        if (other.TryGetComponent(out PhysicsCollectible collectible)) 
        {
            detectedItem = other.gameObject;
            itemDetected = true;
            
            // extra logic according to the feature
            // call any feature on the Player, and use its functions
            // or create a designated method to call the feature from within Player
            // ie MovementHandler()
            
            if (Player.Instance.Inventory.AddItem(collectible.collectable))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhysicsCollectible collectible))
        {
            ResetItemDetector();
        }
    }

    private void ResetItemDetector()
    {
        detectedItem = null;
        itemDetected = false;
    }
}

public class ExampleScript : MonoBehaviour
{
    
}