using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    // private Bus masterBus;
    // private Bus musicBus;
    // private Bus ambienceBus;
    // private Bus sfxBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        // masterBus = RuntimeManager.GetBus("bus:/");
        // musicBus = RuntimeManager.GetBus("bus:/Music");
        // ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        // sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.Music);
    }

    private void Update()
    {
        // masterBus.setVolume(masterVolume);
        // musicBus.setVolume(musicVolume);
        // ambienceBus.setVolume(ambienceVolume);
        // sfxBus.setVolume(SFXVolume);
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private PLAYBACK_STATE PlaybackState(EventInstance instance)
    {
        instance.getPlaybackState(out PLAYBACK_STATE state);
        return state;
    }
    
    public EventInstance? GetInstance(EventInstance eventInstance)
    {
        foreach (var getInstance in eventInstances.Where(getInstance => getInstance.ToString() == eventInstance.ToString()))
        {
            return getInstance;
        }

        return null;
    }
    
    public void PlayEvent(EventInstance fmodEvent, Vector3 posInWorld)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    
    public void PlayEvent(EventInstance fmodEvent, Vector3 posInWorld, string paramName, int paramValue)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.setParameterByName(paramName, paramValue);
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    
    public void StopEvent(EventInstance fmodEvent)
    {
        fmodEvent.stop(STOP_MODE.ALLOWFADEOUT);
        fmodEvent.release();
    }
    
    
    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}