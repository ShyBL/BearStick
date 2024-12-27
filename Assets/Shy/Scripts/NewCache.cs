public class NewCache : OurMonoBehaviour
{
    public void DoCache()
    {
       // Player.Instance.DisableMovement();

       GameManager.GameplayManager.ShopPayoff();
            
       // Invoke(nameof(EndDay),GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.5f);
            
    }

    private void EndDay()
    {
        EndOfDay.Instance.EndDay();
        Player.Instance.EnableMovement();
    }
    
}