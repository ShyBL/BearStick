using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 // Just an extremely simple script for pickups.
 // TODO: The player object will need an inventory system script that includes a list of pickup type.

public class pickup : MonoBehaviour
{
    [SerializeField] private int value;

    [SerializeField] private int size; /* TODO: This may need to be further modified to work with the inventory system.
                                        This could perhaps be reformatted into an array of booleans to illustrate shape?*/

    public int GetValue(){ return value; }

    public int getSize(){ return size; }

    // TODO: Need to better understand food and drink mechanics to add support for this.
}
