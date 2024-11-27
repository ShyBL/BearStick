public class NewShopController : OurMonoBehaviour
{
    private Shop shop;

    private void Start()
    {
        shop = FindFirstObjectByType<Shop>();
    }
        
    public void DoShop()
    {
        Player.Instance.DisableMovement(); 
        Player.Instance.StopInPlace();
        shop.OpenShop();
    }
}