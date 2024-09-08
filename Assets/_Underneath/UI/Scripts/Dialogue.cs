using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class Dialogue : MonoBehaviour
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

    public void StartDialogue(string line, Sprite sprite)
    {
        m_Image.parent.style.display = DisplayStyle.Flex;
        m_Image.style.backgroundImage = new StyleBackground(sprite);
        StartCoroutine(TypeText(line));
    }

    void EndDialogue()
    {
        Player.Instance.playerInput.onDialogueEnd -= EndDialogue;
        m_Image.parent.style.display = DisplayStyle.None;
    }

    IEnumerator TypeText(string line)
    {
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

        Player.Instance.playerInput.onDialogueEnd += EndDialogue;
    }
}
