using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UIElements;

public class Shop : OurMonoBehaviour
{
    [SerializeField]
    List<string> m_DialogueLines = new List<string>();

    UIDocument doc;
    VisualElement m_Root;
    Button m_SellAllButton;
    Button m_TalkButton;
    Button m_ExitButton;
    Dialogue m_Dialogue;
    VisualElement m_DragArea;
    InventoryLayout m_Layout;
    bool m_SellingAll = false;
    private AudioManager audio;
    private GameObject playerObj;
    
    void Start()
    {
        doc = GetComponent<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_SellAllButton = m_Root.Q<Button>("SellAll");
        m_TalkButton = m_Root.Q<Button>("Talk");
        m_ExitButton = m_Root.Q<Button>("Exit");
        m_DragArea = m_Root.Q<VisualElement>("DragArea");
        m_Dialogue = GetComponent<Dialogue>();
        m_Layout = GetComponent<InventoryLayout>();

        m_SellAllButton.RegisterCallback<ClickEvent>(OnSellAllPressed);
        m_TalkButton.RegisterCallback<ClickEvent>(OnTalkButtonPressed);
        m_ExitButton.RegisterCallback<ClickEvent>(OnExitButtonPressed);

        m_Root.style.display = DisplayStyle.None;
        
        audio = GameManager.AudioManager;
        playerObj = FindFirstObjectByType<Player>().gameObject;
    }

    void OnSellAllPressed(ClickEvent evt)
    {
        if(!m_SellingAll)
            StartCoroutine(SellAnimation());
    }

    IEnumerator SellAnimation()
    {
        m_SellingAll = true;

        for (int i = 0; i < Inventory.Instance.StoredItems.Count; i++)
        {
            VisualElement icon = Inventory.Instance.StoredItems[i].RootVisual[m_Layout].Icon;
            VisualElement slot = Inventory.Instance.StoredItems[i].RootVisual[m_Layout];
            icon.parent.Clear();
            m_Root.Add(icon);
            icon.style.width = slot.resolvedStyle.width;
            icon.style.height = slot.resolvedStyle.height;
            icon.transform.position = slot.LocalToWorld(slot.transform.position);
        }

        yield return 0;

        for (int i = 0; i < Inventory.Instance.StoredItems.Count; i++)
        {
            VisualElement icon = Inventory.Instance.StoredItems[i].RootVisual[m_Layout].Icon;

            Vector2 pos = new Vector2(Random.Range(0f, m_DragArea.contentRect.width - icon.contentRect.width), Random.Range(0f, m_DragArea.contentRect.height - icon.contentRect.height));
            icon.transform.position = m_DragArea.LocalToWorld(pos);
        }

        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < Inventory.Instance.StoredItems.Count; i++)
        {
            VisualElement icon = Inventory.Instance.StoredItems[i].RootVisual[m_Layout].Icon;

            icon.parent.Remove(icon);
        }

        PlayerData.Instance.DoCache();
        m_SellingAll = false;
    }

    void OnTalkButtonPressed(ClickEvent evt) 
    {
        m_Dialogue.StartDialogue(m_DialogueLines[Random.Range(0, m_DialogueLines.Count)]);
    }

    void OnExitButtonPressed(ClickEvent evt)
    {
        m_Root.style.display = DisplayStyle.None;
        
        audio.StopEvent(audio.ShopThemeEvent);
        
        audio.PlayEvent(audio.GameplayThemeEvent,playerObj.transform.position);
        
        Player.Instance.EnableMovement();
    }

    public void OpenShop()
    {
        m_Root.style.display = DisplayStyle.Flex;
        audio.PlayEvent(audio.ShopThemeEvent,playerObj.transform.position);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //    OpenShop();
    }
}

