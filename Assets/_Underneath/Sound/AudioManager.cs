using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using System.Threading.Tasks;

public enum ShopType
{
    Theme1, Theme2
}

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

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    public EventInstance FootstepsEvent { get; private set; }
    public EventInstance JumpEvent { get; private set; }
    public EventInstance LandEvent { get; private set; }
    public EventInstance GameplayThemeEvent { get; private set; }
    public EventInstance ShopThemeEvent { get; private set; }
    public EventInstance TempShopThemeEvent { get; private set; }
    
    public EventInstance CrateDragEvent { get; private set; }
    public EventInstance DialogueEvent { get; private set; }
    
    
    private void Awake()
    {
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {
        StartCoroutine(WaitForBanksToLoadCoroutine());
    }

    private IEnumerator WaitForBanksToLoadCoroutine()
    {
        while (!RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }
        InitEventInstances();
    }

    private void InitEventInstances()
    {
        // Music Event Instances
        GameplayThemeEvent = CreateInstance(FMODEvents.Instance.GameplayTheme);
        ShopThemeEvent = CreateInstance(FMODEvents.Instance.ShopTheme);
        
        // Character Event Instances
        FootstepsEvent = CreateInstance(FMODEvents.Instance.Footsteps);
        JumpEvent = CreateInstance(FMODEvents.Instance.Jump);
        LandEvent = CreateInstance(FMODEvents.Instance.Land);
        
        // Gameplay Event Instances
        CrateDragEvent = CreateInstance(FMODEvents.Instance.CrateDrag);
       // DialogueEvent = CreateInstance(FMODEvents.Instance.Dialogue);
        
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayEventWithValueParameters(EventInstance fmodEvent, Vector3 posInWorld, string paramName, int paramValue)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.setParameterByName(paramName, paramValue);
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    
    public void PlayEventWithStringParameters(EventInstance fmodEvent, Vector3 posInWorld, string paramName, string paramValue)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.setParameterByNameWithLabel(paramName, paramValue);
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    
    public void ChangeEventParametersWithString(EventInstance eventInstance, string paramName, string paramValue)
    {
        var fmodEvent = GetInstance(eventInstance);
        fmodEvent.setParameterByNameWithLabel(paramName, paramValue);
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
    
    public EventInstance GetInstance(EventInstance eventInstance)
    {
        foreach (var getInstance in eventInstances.Where(getInstance => getInstance.ToString() == eventInstance.ToString()))
        {
            return getInstance;
        }
        
        return default;
    }
    
    public void PlayEvent(EventInstance fmodEvent, Vector3 posInWorld)
    {
        if (PlaybackState(fmodEvent) != PLAYBACK_STATE.PLAYING)
        {
            fmodEvent.set3DAttributes(RuntimeUtils.To3DAttributes(posInWorld));
            fmodEvent.start();
        }
    }
    

    
    public void StopAndReleaseEvent(EventInstance fmodEvent)
    {
        fmodEvent.stop(STOP_MODE.ALLOWFADEOUT);
        fmodEvent.release();
    }

    public void StopAndDontReleaseEvent(EventInstance fmodEvent)
    {
        fmodEvent.stop(STOP_MODE.ALLOWFADEOUT);
    }
    
    public void PauseEvent(EventInstance fmodEvent)
    {
        fmodEvent.setPaused(true);
    }
    
    public void UnPauseEvent(EventInstance fmodEvent)
    {
        fmodEvent.setPaused(false);
    }
    
    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(STOP_MODE.IMMEDIATE);
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