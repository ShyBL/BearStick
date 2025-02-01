using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Transform lightsParent;
    [SerializeField] private Transform character;
    [SerializeField] private float interpolationSpeed = 1.0f;
    [SerializeField] private float maxDistance = 10.0f; 
    [SerializeField] private float onIntensity = 1.0f; 
    [SerializeField] private float offIntensity = 0.0f;
    private List<Light2D> lights; 
    public UnityEvent<bool> onToggleLights; 
    public UnityEvent onLightsOutScenario; 

    private void Start()
    {
        lights = lightsParent.GetComponentsInChildren<Light2D>().ToList();

        // Add listeners to the Unity events
        onToggleLights.AddListener(toggle => DoLights(lights, toggle));
        onLightsOutScenario.AddListener(LightsOutScenario);
    }

    public void TriggerToggleLights(bool toggle)
    {
        onToggleLights.Invoke(toggle);
    }

    public void TriggerLightsOutScenario()
    {
        onLightsOutScenario.Invoke();
    }

    private async void DoLights(List<Light2D> lights, bool toggle)
    {
        // Create and await tasks for toggling each light
        var toggleTasks = lights.Select(light => ToggleLight(light, toggle));
        await Task.WhenAll(toggleTasks);
    }

    private async Task ToggleLight(Light2D lightComponent, bool toggle)
    {
        // Determine target intensity based on the toggle parameter
        float targetIntensity = toggle ? onIntensity : offIntensity; 
        float initialIntensity = lightComponent.intensity;

        // Interpolate intensity over time
        float elapsedTime = 0.0f;
        while (elapsedTime < interpolationSpeed)
        {
            lightComponent.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / interpolationSpeed);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }

        // Ensure final intensity is set
        lightComponent.intensity = targetIntensity;
    }

    private async void LightsOutScenario()
    {
        // Filter lamp posts and window lights using LINQ
        var lampPosts = lights.Where(l => l.lightType == Light2D.LightType.Point).ToList();
        var windowLights = lights.Where(l => l.lightType == Light2D.LightType.Sprite).ToList();

        // Create and await tasks for toggling lamp posts
        var lampTasks = lampPosts.Select(async lampPost =>
        {
            float distance = Vector2.Distance(character.position, lampPost.transform.position);
            bool shouldTurnOff = distance < maxDistance;

            await ToggleLight(lampPost, !shouldTurnOff);
        });

        await Task.WhenAll(lampTasks);

        // Create and await tasks for toggling window lights based on lamp post status
        var windowTasks = windowLights.Select(windowLight =>
        {
            bool isLitByLampPost = lampPosts.Any(lp => 
                Vector2.Distance(windowLight.transform.position, lp.transform.position) < maxDistance && lp.intensity > 0.0f);

            return ToggleLight(windowLight, !isLitByLampPost);
        });

        await Task.WhenAll(windowTasks);
    }
}
