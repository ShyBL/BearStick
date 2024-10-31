using UnityEngine;

public class PhysicsCollectible : MonoBehaviour
{
    [SerializeField] private float launchForce;
    [SerializeField] private float randomFactor;
    
    public Item collectable;
    private Rigidbody2D rigidBody;
    
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Launch();
    }

    private void Launch()
    {
        float randomX = Random.Range(-randomFactor, randomFactor);
        float randomY = Random.Range(0.8f, 1f) * launchForce;
        
        Vector2 launchDirection = new Vector2(randomX, randomY).normalized;
        
        rigidBody.velocity = launchDirection * launchForce;
    }

    public void SetCollectable(Item inputCollectable)
    {
        collectable = inputCollectable;

        GetComponent<SpriteRenderer>().sprite = collectable.Collectable;
    }
}
