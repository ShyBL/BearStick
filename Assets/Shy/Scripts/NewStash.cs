using System;
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
    public bool Opened;
    public void DoStash()
    {
        if (Opened) return;
        
        Player.Instance.DisableMovement(); 

        foreach (Item collectable in collectableList)
        {
            PhysicsCollectible newCollectable = Instantiate(physicsCollectable, OutPoint.position, quaternion.identity).GetComponent<PhysicsCollectible>();
            newCollectable.SetCollectable(collectable);
        }
        
        GetComponentInChildren<LineRenderer>().enabled = false;
        Opened = true;
        Player.Instance.EnableMovement();

    }
}