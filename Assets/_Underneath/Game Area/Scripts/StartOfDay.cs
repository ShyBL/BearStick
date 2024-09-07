using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOfDay : MonoBehaviour
{
    public static StartOfDay Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SavingAndLoading.Instance.CheckIfFileExistsOnStart();
    }

    public void StartNewDay()
    {
        Debug.Log("Starting a New Day...");
        //Load Player Information
        SavingAndLoading.Instance.LoadPlayerInformation();
        //Sets the player location to a point in the world, we can set that to whereever we need
        Player.Instance.SetPlayerSpawn(PlayerData.Instance.v_SpawnLocation);
        //Fade out from black

        //Start Timer
        CurfewTimer.Instance.StartTimer();
    }
}
