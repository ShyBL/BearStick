using TMPro;
using UnityEngine;

public class ExampleObject : MonoBehaviour
{
    [SerializeField] private PickupData data; // we can drag in Unity, any data class, into this field to use its definition
    private TextMeshProUGUI nameText; // this a reference for 2D text in the scene
    
    // This is an example of how the data can be used
    void Start()
    {
        // Assign the name from the data, to the text
        // Access the text component within the TextMeshProUGUI, it can be equal to any string value
        nameText.text = data.GetName();
    }
}
