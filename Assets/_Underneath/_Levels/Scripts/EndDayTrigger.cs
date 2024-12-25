using UnityEngine;

public class EndDayTrigger : MonoBehaviour
{
    [SerializeField] private GameObject StartDayTrigger;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (GameplayManager.Instance.GetDayCount() != 0)
            {
                player.StopInPlace();
                player.DisableMovement();
                CurfewTimer.Instance.bPlayerHasLeftBase = false;
                EndOfDay.Instance.EndDay();
                Debug.Log("Inside safe zone, auto end of day ");
                
                StartDayTrigger.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}