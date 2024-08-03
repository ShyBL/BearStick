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

    [Header("Cam Constraints")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // Update is called once per frame
    private void FixedUpdate()
    {
        float xClamp = Mathf.Clamp(racc.position.x, xMin, xMax);
        float yClamp = Mathf.Clamp(racc.position.y, yMin, yMax);

        if (Mathf.Abs(camSpot.x - racc.position.x) > deadZone.x)
        {
            getCamPos();
            Vector3 fixedPos = new Vector3(Mathf.Clamp(camSpot.x, xMin, xMax), Mathf.Clamp(camSpot.y, yMin, yMax), offset.z);
            //transform.position = camSpot;
            transform.position = fixedPos;
        }
        else if (Mathf.Abs(camSpot.y - racc.position.y) > deadZone.y)
        {
            getCamPos();
            Vector3 fixedPos = new Vector3(Mathf.Clamp(camSpot.x, xMin, xMax), Mathf.Clamp(camSpot.y, yMin, yMax), offset.z);
            //transform.position = camSpot;
            transform.position = fixedPos;
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
