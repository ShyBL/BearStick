using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOfDay : MonoBehaviour
{
    public static StartOfDay Instance;
    [SerializeField] float speedScale = 1f;
    [SerializeField] Color fadeColor = Color.black;
    [SerializeField] AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0, 1), 
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1,0));

    private float alpha;
    private Texture2D texture;
    private int direction = 0;
    [SerializeField]  private float time = 0f;

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
        ResetFadeAnimation();
    }

    private void Update()
    {
        if(direction == 0)
        {
            if(alpha >= 1f) //Fully faded out 
            {
                alpha = 1f;
                time = 0f;
                direction = 1;
            }
            else    //Fully faded in
            {
                alpha = 0f;
                time = 1f;
                direction = 1;
            }
        }
    }

    private void ResetFadeAnimation()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeColor.a));
        direction = 0;
        alpha = 1f;
        texture.Apply();
    }

    public void StartNewDay()
    {
        Debug.Log("Starting a New Day...");
        //Load the scene again *For Now*
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Load Player Information
        SavingAndLoading.Instance.LoadPlayerInformation();
        //Sets the player location to a point in the world, we can set that to whereever we need
        Player.Instance.SetPlayerSpawn(PlayerData.Instance.v_SpawnLocation);
        //Fade out from black
        ResetFadeAnimation();
        //Start Timer
        CurfewTimer.Instance.StartTimer();


    }

    public void OnGUI()
    {
        if (alpha > 0f)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
        if (direction != 0) 
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = fadeCurve.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if(alpha <= 0f || alpha >= 1f) 
            {
                direction = 0;
            }
        }
    }
}
