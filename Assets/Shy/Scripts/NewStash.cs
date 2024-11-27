using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NewStash : OurMonoBehaviour
{
    [SerializeField] 
    private List<Item> collectableList;
    [SerializeField] 
    private GameObject physicsCollectable;
    [SerializeField] private Transform OutPoint;
    public bool opened = false;
    
    public void DoStash()
    {
        if (opened) return;
        
        Player.Instance.DisableMovement(); 

        foreach (Item collectable in collectableList)
        {
            PhysicsCollectible newCollectable = Instantiate(physicsCollectable, OutPoint.position, quaternion.identity).GetComponent<PhysicsCollectible>();
            newCollectable.SetCollectable(collectable);
        }

        opened = true;
        Player.Instance.EnableMovement();

    }
}