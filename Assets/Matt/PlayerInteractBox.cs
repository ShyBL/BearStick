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
            
            if (Inventory.AddItem(other.GetComponent<PhysicsCollectible>().collectable))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
