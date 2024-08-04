using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    private UIDocument m_Doc;
    private VisualElement m_Root;
    private Button m_Play;
    private Button m_Exit;
    private VideoPlayer m_Player;

    // Start is called before the first frame update
    void Start()
    {
        m_Doc = GetComponent<UIDocument>();
        m_Player = GetComponent<VideoPlayer>();
        m_Root = m_Doc.rootVisualElement;
        m_Play = m_Root.Q<Button>("Play");
        m_Exit = m_Root.Q<Button>("Exit");
        
        m_Play.RegisterCallback<ClickEvent>(PlayPressed);
        m_Exit.RegisterCallback<ClickEvent>(ExitPressed);
    }

    void PlayPressed(ClickEvent evt)
    {
        m_Root.style.display = DisplayStyle.None;
        m_Player.loopPointReached += VideoEnded;
        m_Player.Play();
    }

    void VideoEnded(VideoPlayer play)
    {
        SceneManager.LoadScene("FullTestScene");
    }

    void ExitPressed(ClickEvent evt)
    {
        Application.Quit();
    }
}
