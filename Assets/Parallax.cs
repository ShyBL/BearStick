using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    private GameObject cam;
    public float parallaxEffect; // Higher values will make the layer move faster, simulating it being closer to the camera.

    void Start()
    {
        cam = Camera.main.gameObject;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}