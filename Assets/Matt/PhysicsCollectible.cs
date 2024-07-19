using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PhysicsCollectible : MonoBehaviour
{
    private Collectable collectable;
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
        float launchForce = 10f;
        float randomFactor = 2f;

        float randomX = Random.Range(-randomFactor, randomFactor);
        float randomY = Random.Range(0.8f, 1f) * launchForce;

        Vector2 launchDirection = new Vector2(randomX, randomY).normalized;

        rigidBody.velocity = launchDirection * launchForce;
    }

    public void SetCollectable(Collectable inputCollectable)
    {
        collectable = inputCollectable;
    }
}
