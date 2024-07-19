using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            if (Inventory.AddItem(collectable))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
