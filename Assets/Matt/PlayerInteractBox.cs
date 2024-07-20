using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractBox : MonoBehaviour
{
    [SerializeField] private Inventory Inventory; // have to insert in the inspector
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log($"Picked a {other.gameObject.name}");
            
            var collectible = other.GetComponent<PhysicsCollectible>();
            
            if (Inventory.AddItem(collectible.collectable)) // if you have space
            {
                Destroy(other.gameObject);
            }
        }
    }
}
