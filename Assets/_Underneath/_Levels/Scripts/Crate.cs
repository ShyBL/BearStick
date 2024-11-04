using System;
using UnityEngine;

    public class Crate : OurMonoBehaviour
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
                GameManager.AudioManager.PlayEventWithValueParameters
                ( GameManager.AudioManager.CrateDragEvent, 
                    gameObject.transform.position,
                    "End", 0);
            }
            else if (rb.velocity.x == 0)
            {
                GameManager.AudioManager.PlayEventWithValueParameters
                ( GameManager.AudioManager.CrateDragEvent, 
                    gameObject.transform.position,
                    "End", 1);
            }
        }
    }
