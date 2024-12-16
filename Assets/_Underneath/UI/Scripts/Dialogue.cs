using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class Dialogue : OurMonoBehaviour
{
    public float CharactersPerSecond;

    UIDocument m_Document;
    VisualElement m_Root;
    Label m_Text;
    VisualElement m_Image;

    private void Awake()
    {
        m_Document = GetComponent<UIDocument>();
        m_Root = m_Document.rootVisualElement;
        m_Text = m_Root.Q<Label>("Dialogue");
        m_Image = m_Root.Q<VisualElement>("Image");
        m_Image.parent.style.display = DisplayStyle.None;
    }

    public void StartDialogue(string line, string speaker, Sprite sprite = null)
    {
        m_Image.parent.style.display = DisplayStyle.Flex;
        if (sprite == null)
            m_Image.style.display = DisplayStyle.None;
        else
        {
            m_Image.style.display = DisplayStyle.Flex;
            m_Image.style.backgroundImage = new StyleBackground(sprite);
        }
        StartCoroutine(TypeText(line,speaker));
    }

    void EndDialogue()
    {
        Player.Instance.playerInput.onDialogueEnd -= EndDialogue;
        m_Image.parent.style.display = DisplayStyle.None;
    }

    IEnumerator TypeText(string line, string speaker)
    {
        GameManager.AudioManager.PlayEventWithStringParameters(GameManager.AudioManager.DialogueEvent,transform.position,"Character",speaker);
        
        float timer = 0;
        float interval = 1 / CharactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                m_Text.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        GameManager.AudioManager.StopAndDontReleaseEvent(GameManager.AudioManager.DialogueEvent);

        Player.Instance.playerInput.onDialogueEnd += EndDialogue;
    }
    
    // public float CharactersPerSecond;
    //
    // UIDocument m_Document;
    // VisualElement m_Root;
    // Label m_Text;
    // VisualElement m_DialogueImage;
    //
    // Label m_GossipText;
    // VisualElement m_GossipBackground;
    // private void Awake()
    // {
    //     m_Document = GetComponent<UIDocument>();
    //     m_Root = m_Document.rootVisualElement;
    //     m_Text = m_Root.Q<Label>("Dialogue");
    //     m_DialogueImage = m_Root.Q<VisualElement>("DialogueImage");
    //     m_DialogueImage.parent.style.display = DisplayStyle.None;
    //     
    //     m_GossipText = m_Root.Q<Label>("Gossip"); // TODO - Make sure these are added to the UXML
    //     m_GossipBackground = m_Root.Q<VisualElement>("GossipBackground"); // TODO - Make sure these are added to the UXML
    //     m_GossipBackground.parent.style.display = DisplayStyle.None;
    // }
    //
    // public void StartDialogue(string line, Sprite sprite = null)
    // {
    //     m_DialogueImage.parent.style.display = DisplayStyle.Flex;
    //     if (sprite == null)
    //         m_DialogueImage.style.display = DisplayStyle.None;
    //     else
    //     {
    //         m_DialogueImage.style.display = DisplayStyle.Flex;
    //         m_DialogueImage.style.backgroundImage = new StyleBackground(sprite);
    //     }
    //     StartCoroutine(TypeText(line));
    // }
    //
    // public void StartGossip(string line, Vector3 worldPosition)
    // {
    //     Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition); // Convert to screen space
    //     m_GossipText.style.left = screenPosition.x;
    //     m_GossipText.style.top = screenPosition.y;
    //     m_GossipText.style.display = DisplayStyle.Flex;
    //     
    //     StartCoroutine(TypeGossipText(line));
    // }
    //
    // void EndDialogue()
    // {
    //     Player.Instance.playerInput.onDialogueEnd -= EndDialogue;
    //     m_DialogueImage.parent.style.display = DisplayStyle.None;
    // }
    //
    // IEnumerator TypeText(string line)
    // {
    //     float timer = 0;
    //     float interval = 1 / CharactersPerSecond;
    //     string textBuffer = null;
    //     char[] chars = line.ToCharArray();
    //     int i = 0;
    //
    //     while (i < chars.Length)
    //     {
    //         if (timer < Time.deltaTime)
    //         {
    //             textBuffer += chars[i];
    //             m_Text.text = textBuffer;
    //             timer += interval;
    //             i++;
    //         }
    //         else
    //         {
    //             timer -= Time.deltaTime;
    //             yield return null;
    //         }
    //     }
    //
    //     Player.Instance.playerInput.onDialogueEnd += EndDialogue;
    // }
    //
    // IEnumerator TypeGossipText(string line)
    // {
    //     float timer = 0;
    //     float interval = 1 / CharactersPerSecond;
    //     string textBuffer = null;
    //     char[] chars = line.ToCharArray();
    //     int i = 0;
    //
    //     while (i < chars.Length)
    //     {
    //         if (timer < Time.deltaTime)
    //         {
    //             textBuffer += chars[i];
    //             m_GossipText.text = textBuffer;
    //             timer += interval;
    //             i++;
    //         }
    //         else
    //         {
    //             timer -= Time.deltaTime;
    //             yield return null;
    //         }
    //     }
    //     
    //     m_GossipText.style.display = DisplayStyle.None;
    // }
}