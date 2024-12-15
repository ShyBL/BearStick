using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OurGameManager : MonoBehaviour
{
    public static OurGameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Two {typeof(OurGameManager)} instances exist, didn't create new one");
            return;
        }
    }

    private void Start()
    {
#if UNITY_WEBGL
        StartCoroutine(InitGameManagerCoroutine());
#else
            var task = InitGameManagerAsync();
            if (task.IsCompleted)
            {
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            }
#endif
    }

#if UNITY_WEBGL
    private IEnumerator InitGameManagerCoroutine()
    {
        yield return StartCoroutine(InitManagersCoroutine());
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private IEnumerator InitManagersCoroutine()
    {
        AudioManager = FindFirstObjectByType<AudioManager>();
        yield return null; // Ensure the coroutine yields at least once
    }
#else
    private async Task InitGameManagerAsync()
    { 
        await InitManagersAsync();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private Task InitManagersAsync()
    {
        AudioManager = FindFirstObjectByType<AudioManager>();
        return Task.CompletedTask;
    }
#endif

    private OpeningCutscene _openingCutscene;
    public AudioManager AudioManager;
    
    public PlayerData PlayerData;
    public SavingAndLoading SavingAndLoading;
    
    public EndOfDay EndOfDay;
    public CurfewTimer CurfewTimer;
    public StartOfDay StartOfDay;

    public TutorialManager TutorialManager;
    public Inventory Inventory;
}