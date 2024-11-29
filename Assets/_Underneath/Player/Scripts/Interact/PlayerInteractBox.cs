using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log($"Picked a {other.gameObject.name}");
            
            var collectible = other.GetComponent<PhysicsCollectible>();
            
            if (Inventory.Instance.AddItem(collectible.collectable)) // if you have space
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
