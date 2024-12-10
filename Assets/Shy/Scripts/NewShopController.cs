public class NewShopController : OurMonoBehaviour
{
    private Shop shop;
    private AudioManager audio;

    private void Start()
    {
        shop = FindFirstObjectByType<Shop>();
        audio = GameManager.AudioManager;
    }
        
    public void DoShop()
    {
        Player.Instance.DisableMovement(); 
        Player.Instance.StopInPlace();
        
        audio.PauseEvent(audio.GameplayThemeEvent);
        
        shop.OpenShop();
    }
}