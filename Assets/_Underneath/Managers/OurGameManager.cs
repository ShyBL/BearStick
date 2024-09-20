using System;
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
        var task = InitGameManager();
        if (task.IsCompleted)
        {
            SceneManager.LoadSceneAsync("OpeningCutscene", LoadSceneMode.Additive);
        }
    }

    private async Task InitGameManager()
    { 
        await InitManagers();
    }

    private Task InitManagers()
    {
        AudioManager = FindFirstObjectByType<AudioManager>();
        
        return Task.CompletedTask;
    }

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