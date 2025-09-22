using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class EnableAtStart : MonoBehaviour
{

    public List<GameObject> objectsToEnable;

    // Start is called before the first frame update
    void Awake()
    {
        EnableObjects();
    }

    void EnableObjects()
    {
        foreach (var obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
