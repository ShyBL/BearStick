using UnityEngine;

public class EndDayTrigger : OurMonoBehaviour
{
    [SerializeField] private GameObject StartDayTrigger;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (GameManager.GameplayManager.GetDayCount() != 0)
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