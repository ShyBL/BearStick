using System;
using UnityEngine;

    public class Crate : MonoBehaviour
    {
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (rb.velocity.x != 0)
            {
                AudioManager.Instance.PlayEventWithValueParameters
                (AudioManager.Instance.CrateDragEvent, 
                    gameObject.transform.position,
                    "End", 0);
            }
            else if (rb.velocity.x == 0)
            {
                AudioManager.Instance.PlayEventWithValueParameters
                (AudioManager.Instance.CrateDragEvent, 
                    gameObject.transform.position,
                    "End", 1);
            }
        }
    }
