using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelTrigger : MonoBehaviour
{
    [SerializeField] private GameObject EndDayTrigger;
    
     private void OnTriggerExit2D(Collider2D collision)
    {   
        if (collision.TryGetComponent(out Player player))
        {
            CurfewTimer.Instance.bPlayerHasLeftBase = true;
            
            PlayerData.Instance.IncrementDayCount();
            CurfewTimer.Instance.StartTimer();

            Debug.Log("Timer Started");
            
            EndDayTrigger.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}