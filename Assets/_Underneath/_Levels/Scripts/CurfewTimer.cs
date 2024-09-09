using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CurfewTimer : MonoBehaviour
{
    public static CurfewTimer Instance;
    [SerializeField] TMP_Text m_CountdownText;
    [SerializeField] private float Countdowntimer, StartingTimer;
    private float timeChangeLight, timeChangeMusic;
    [SerializeField, Range(0, 1)] private float percentToChangeLights;
    [SerializeField, Range(0, 1)] private float percentToChangeMusic;
    public float threshold = 0.5f;
    private bool bIsTimerDone, bLightsChanged, bMusicChanged;
    [SerializeField] private bool bResetScene = false;
    [SerializeField] TimerComponent m_UITimer;

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

        bIsTimerDone = true;
        bLightsChanged = false;
        bMusicChanged = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        StartTimer();
        Debug.Log("Timer Started");
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsTimerDone)
        {
            if(Countdowntimer > 0)
            {
                Countdowntimer -= Time.deltaTime;
                m_UITimer.TimeRemaining = Countdowntimer;
            }
            else if (Countdowntimer <= 0)
            {
                bIsTimerDone = true;
                Debug.Log("Timer Ended");
                //Can comment this part out if we want to not go to EndDay
                TimerEnded();
                //Start EndOfDay stuff
            }

            if(Countdowntimer <= timeChangeLight + threshold && !bLightsChanged)
            {
                ChangeLights();
            }
            if (Countdowntimer <= timeChangeMusic + threshold && !bMusicChanged)
            {
                ChangeMusic();
            }
            //Can use this when you need it to visually represent how much time is left in the day
            int mins = Mathf.FloorToInt(Countdowntimer / 60);
            int sec = Mathf.FloorToInt(Countdowntimer % 60);
            //m_CountdownText.text = string.Format("{0:00}:{1:00}", mins, sec);
        }
    }

    public float GetTimeRemaining()
    {
        //Can do something like a check on how much time is remaining
        return Countdowntimer;
    }

    public void StartTimer()
    {
        Countdowntimer = StartingTimer;
        m_UITimer.TimeLimit = StartingTimer;
        timeChangeLight = Countdowntimer * percentToChangeLights;
        timeChangeMusic = Countdowntimer * percentToChangeMusic;
        //Add inventory Timer stuff

        //Everything That TimerEnded turns off needs to be turned back on.
        bIsTimerDone = false;
        Time.timeScale = 1.0f;
        Player.Instance.EnableMovement();
    }

    public void PauseTimer()
    {
        Time.timeScale = 0.0f;
        Player.Instance.DisableMovement();
    }

    public void ResumeTimer()
    {
        Time.timeScale = 1.0f;
        Player.Instance.EnableMovement();
    }

    private void ChangeMusic()
    {
        bMusicChanged = true;
        Debug.Log("Change Music Called");
    }

    private void ChangeLights()
    {
        bLightsChanged = true;
        Debug.Log("Change Lights Called");
    }


    public void TimerEnded()
    {
        Debug.Log("Timer has ended");
        bIsTimerDone = true;

        //STOP EVERYTHING FOR SCENE TO PAUSE FUNCTIONALITY
        Time.timeScale = 0.0f;
        Player.Instance.DisableMovement();
        EndOfDay.Instance.EndDay();
        //Maybe the EndDay function should handle all the pausing of stuff? just a thought
        //For Testing
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    /*
     THINGS TO STOP
        Player Movement
        Music
        *Add more here*
        *
     Everything that is stopped when the timer is up would have to be reset and started again
    */
}
