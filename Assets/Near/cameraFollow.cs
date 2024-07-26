using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector2 smoothTime;
    [SerializeField] private Transform racc;
    [SerializeField] private Vector2 deadZone;

    private Vector3 camSpot;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Mathf.Abs(camSpot.x - racc.position.x) > deadZone.x)
        {
            getCamPos();
            transform.position = camSpot;
        }
        else if (Mathf.Abs(camSpot.y - racc.position.y) > deadZone.y)
        {
            getCamPos();
            transform.position = camSpot;
        }
    }

    private void getCamPos()
    {
        camSpot.x = Mathf.Lerp(transform.position.x, racc.position.x + offset.x, Time.deltaTime * smoothTime.x);

        camSpot.y = Mathf.Lerp(transform.position.y, racc.position.y + offset.y, Time.deltaTime * smoothTime.y);

        if (camSpot.x - racc.position.x > deadZone.x)
        {
            camSpot.x = racc.position.x + deadZone.x;
        }
        else if (camSpot.x - racc.position.x < -deadZone.x)
        {
            camSpot.x = racc.position.x - deadZone.x;
        }
        else
        {
            camSpot.x = camSpot.x;
        }

        if (camSpot.y - racc.position.y > deadZone.y)
        {
            camSpot.y = racc.position.y + deadZone.y;
        }
        else if (camSpot.y - racc.position.y < -deadZone.y)
        {
            camSpot.y = racc.position.y - deadZone.y;
        }
        else
        {
            camSpot.y = camSpot.y;
        }

        camSpot.z = offset.z;
    }
}
