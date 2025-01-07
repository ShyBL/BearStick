using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference MusicManager { get; private set; }
    

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference Footsteps { get; private set; }
    [field: SerializeField] public EventReference Jump { get; private set; }
    [field: SerializeField] public EventReference Land { get; private set; }

    [field: SerializeField] public EventReference OpenBag { get; private set; }

    [field: Header("Item SFX")]
    [field: SerializeField] public EventReference CollectedItem { get; private set; }
    [field: SerializeField] public EventReference CrateDrag { get; private set; }

    [field: Header("Area SFX")]
    [field: SerializeField] public EventReference Dumpster { get; private set; }
    [field: SerializeField] public EventReference Trashcan { get; private set; }
    [field: SerializeField] public EventReference Stash { get; private set; }
    [field: SerializeField] public EventReference Dialogue { get; private set; }

    
    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference Click { get; private set; }
    [field: SerializeField] public EventReference Pause { get; private set; }
    [field: SerializeField] public EventReference UnPause { get; private set; }
    [field: SerializeField] public EventReference Handwriting { get; private set; }

    public static FMODEvents Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        Instance = this;
    }
    

}