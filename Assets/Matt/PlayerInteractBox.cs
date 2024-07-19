using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractBox : MonoBehaviour
{
    [SerializeField] private Inventory Inventory;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log($"Picked a {other.gameObject.name}");
            
            Destroy(other.gameObject);
            var Collectible = other.GetComponent<PhysicsCollectible>();
            if (Inventory.AddItem(Collectible.collectable))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
