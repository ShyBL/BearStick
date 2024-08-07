using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractBox : MonoBehaviour
{
    private bool tutorialDoOnce = true;
    //[SerializeField] private Inventory Inventory; // have to insert in the inspector
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            // if (tutorialDoOnce)
            // {
            //     TutorialManager.Instance.carrying = true;
            //     tutorialDoOnce = false;
            // }
            
            Debug.Log($"Picked a {other.gameObject.name}");
            
            var collectible = other.GetComponent<PhysicsCollectible>();
            
            if (Player.Instance.inventory.AddItem(collectible.collectable)) // if you have space
            {
                Destroy(other.gameObject);
            }
        }
    }
}
