using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CurfewTimer : MonoBehaviour
{
    public static CurfewTimer Instance;

    [SerializeField] TMP_Text m_CountdownText;
    [SerializeField] private float Countdowntimer;
    private bool bIsTimerDone = false;
    [SerializeField] private bool bResetScene = false;

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
    // Update is called once per frame
    void Update()
    {
        if (!bIsTimerDone)
        {
            if(Countdowntimer > 0)
            {
                Countdowntimer -= Time.deltaTime;
            }
            else if (Countdowntimer <= 0)
            {
                bIsTimerDone = true;
                Debug.Log("Timer Ended");
                //Time.timeScale = 0.0f;
                //Start EndOfDay stuff
            }
            //Can use this when you need it to visually represent how much time is left in the day
            int mins = Mathf.FloorToInt(Countdowntimer / 60);
            int sec = Mathf.FloorToInt(Countdowntimer % 60);
            //m_CountdownText.text = string.Format("{0:00}:{1:00}", mins, sec);
            //Debug.Log(Countdowntimer.ToString());
        }
    }

    public float GetTimeRemaining()
    {
        //Can do something like a check on how much time is remaining
        return Countdowntimer;
    }

    public void StartTimer(float timerAmountInSeconds)
    {
        Countdowntimer = timerAmountInSeconds;
        //Everything That TimerEnded turns off needs to be turned back on.
        bIsTimerDone = false;
        Time.timeScale = 1.0f;
        Player.Instance.EnableMovement();

        Invoke("TimerEnded", Countdowntimer);
    }

    public void TimerEnded()
    {
        Debug.Log("Timer has ended");
        bIsTimerDone = true;

        //STOP EVERYTHING FOR SCENE TO PAUSE FUNCTIONALITY
        Time.timeScale = 0.0f;
        Player.Instance.DisableMovement();
        EndOfDay.Instance.EndDay();
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
