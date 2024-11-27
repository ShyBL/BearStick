using System;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Vector3 movementScale = Vector3.one;
    [SerializeField] private Camera _camera;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraMovement = Vector3.Scale(_camera.transform.position, movementScale);
        Vector3 newPosition = initialPosition + new Vector3(cameraMovement.x, 0, 0); 
        transform.position = newPosition;
    }
}