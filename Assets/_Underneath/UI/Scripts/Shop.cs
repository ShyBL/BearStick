using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    [SerializeField]
    List<string> m_DialogueLines = new List<string>();

    UIDocument doc;
    VisualElement m_Root;
    Button m_SellAllButton;
    Button m_TalkButton;
    Button m_ExitButton;
    Dialogue m_Dialogue;

    void Start()
    {
        doc = GetComponent<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_SellAllButton = m_Root.Q<Button>("SellAll");
        m_TalkButton = m_Root.Q<Button>("Talk");
        m_ExitButton = m_Root.Q<Button>("Exit");
        m_Dialogue = GetComponent<Dialogue>();

        m_SellAllButton.RegisterCallback<ClickEvent>(OnSellAllPressed);
        m_TalkButton.RegisterCallback<ClickEvent>(OnTalkButtonPressed);
        m_ExitButton.RegisterCallback<ClickEvent>(OnExitButtonPressed);

        m_Root.style.display = DisplayStyle.None;
    }

    void OnSellAllPressed(ClickEvent evt)
    {
        PlayerData.Instance.DoCache();
    }

    void OnTalkButtonPressed(ClickEvent evt) 
    {
        m_Dialogue.StartDialogue(m_DialogueLines[Random.Range(0, m_DialogueLines.Count)]);
    }

    void OnExitButtonPressed(ClickEvent evt)
    {
        m_Root.style.display = DisplayStyle.None;
        var audio = AudioManager.Instance;
        
        switch (audio.themeType)
        {
            case ShopType.Theme1:
                audio.StopEvent(audio.ShopThemeEvent);
                break;
            case ShopType.Theme2:
                audio.StopEvent(audio.TempShopThemeEvent);
                break;
        }
    }

    public void OpenShop()
    {
        m_Root.style.display = DisplayStyle.Flex;
        var audio = AudioManager.Instance;
        switch (audio.themeType)
        {
            case ShopType.Theme1:
                audio.PlayEvent(audio.ShopThemeEvent,audio.gameObject.transform.position);
                break;
            case ShopType.Theme2:
                audio.PlayEvent(audio.TempShopThemeEvent,audio.gameObject.transform.position);
                break;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            OpenShop();
    }
}

