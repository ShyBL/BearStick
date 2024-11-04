using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelTrigger : MonoBehaviour
{
    // private void OnTriggerEnter2D(Collider2D collision)
    // {   
    //     if (collision.TryGetComponent(out Player player))
    //     {
    //         if (PlayerData.Instance.GetDayCount() != 0)
    //         {
    //             CurfewTimer.Instance.bPlayerHasLeftBase = false;
    //             EndOfDay.Instance.EndDay();
    //             Debug.Log("Inside safe zone, auto end of day ");
    //         }
    //         
    //     }
    // }
     private void OnTriggerExit2D(Collider2D collision)
    {   
        if (collision.TryGetComponent(out Player player))
        {
            CurfewTimer.Instance.bPlayerHasLeftBase = true;
            
            PlayerData.Instance.IncrementDayCount();
            CurfewTimer.Instance.StartTimer();

            Debug.Log("Timer Started");
        }
    }
}
