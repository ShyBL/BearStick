using System;
using System.Collections;
using System.Threading.Tasks;  // For async/await support
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class OpeningCutscene : MonoBehaviour
{
    [Header("Define Me!")]
    [SerializeField] private float CharactersPerSecond;
    [SerializeField] private float normalDelay;
    [SerializeField] private float extraDelay;
    [SerializeField] private float longDelay;

    UIDocument m_Document;
    VisualElement m_Root;
    Label m_Text1;
    Label m_Text2;
    Label m_Text3;
    VisualElement m_Image;

    private void Awake()
    {
        m_Document = GetComponent<UIDocument>();
        m_Root = m_Document.rootVisualElement;
        m_Text1 = m_Root.Q<Label>("FirstLine");
        m_Text2 = m_Root.Q<Label>("SecondLine");
        m_Text3 = m_Root.Q<Label>("LastLine");
        m_Image = m_Root.Q<VisualElement>("Photograph");

        //SetLabelDisplay(m_Text1, DisplayStyle.None);
        //SetLabelDisplay(m_Text2, DisplayStyle.None);
        //SetLabelDisplay(m_Text3, DisplayStyle.None);
        //SetVisualElementDisplay(m_Image, DisplayStyle.None);

        RunCutscene();
    }

    private async void RunCutscene()
    {
        await StartDialogue(normalDelay, extraDelay, longDelay, ". . .");
        await StartDialogue(normalDelay, extraDelay, longDelay, 
            "To Luca, Rowan, and Asher", 
            "How is everything at Mrs. Walker’s house?", 
            "I hope you’ve made some friends with other kids there");
        await StartDialogue(normalDelay, extraDelay, longDelay, 
            "I can only imagine how big you’re getting", 
            "and I know you have a lot of questions", 
            "and I’ll answer all of them when I’m back.");
        await StartDialogue(normalDelay, extraDelay, longDelay, 
            "Sometimes in life, we have to make hard decisions in order to do what’s right", 
            "and nothing was harder for me then having to leave the three of you.");
        await StartDialogue(normalDelay, extraDelay, longDelay, 
            "I hope you understand that this is what’s best for our family", 
            "For you three", "I need you to be brave for just a little bit longer.");
        await StartDialogue(normalDelay, extraDelay, longDelay, 
            "I’ll see you soon, my loves", String.Empty, "-Mom", true);
    }

    private async Task StartDialogue(float delay1Line, float delay2Line, float delay3Line, string line1 = null, string line2 = null, string line3 = null, bool photo = false)
    {
        m_Image.style.display = photo ? DisplayStyle.Flex : DisplayStyle.None;

        await DisplayLines(line1, line2, line3, delay1Line, delay2Line, delay3Line);
    }

    private async Task DisplayLines(string line1, string line2, string line3, float delay1, float delay2, float delay3)
    {
        if (!string.IsNullOrEmpty(line1))
        {
            await TypeText(line1, m_Text1);
            await Task.Delay(TimeSpan.FromSeconds(delay1));
            await FadeLabel(m_Text1, 1f, 0f, 1f);
        }

        if (!string.IsNullOrEmpty(line2))
        {
            await TypeText(line2, m_Text2);
            await Task.Delay(TimeSpan.FromSeconds(delay2));
            await FadeLabel(m_Text2, 1f, 0f, 1f);
        }

        if (!string.IsNullOrEmpty(line3))
        {
            await TypeText(line3, m_Text3);
            await Task.Delay(TimeSpan.FromSeconds(delay3));
            await FadeLabel(m_Text3, 1f, 0f, 1f);
        }


        SetLabelAlpha(m_Text1, 0f);
        SetLabelAlpha(m_Text2, 0f);
        SetLabelAlpha(m_Text3, 0f);
        
        //SetLabelDisplay(m_Text1, DisplayStyle.None);
        //SetLabelDisplay(m_Text2, DisplayStyle.None);
        //SetLabelDisplay(m_Text3, DisplayStyle.None);
    }

    private async Task TypeText(string line, Label label)
    {
        //SetLabelDisplay(label, DisplayStyle.Flex);
        SetLabelAlpha(label, 1f);

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
                label.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                await Task.Yield(); 
            }
        }
    }

    private void SetLabelDisplay(Label label, DisplayStyle displayStyle)
    {
        label.style.display = displayStyle;
    }

    private void SetVisualElementDisplay(VisualElement visualElement, DisplayStyle displayStyle)
    {
        visualElement.style.display = displayStyle;
    }
    
    private void SetLabelAlpha(Label label, float alpha)
    {
        label.style.opacity = alpha;
    }

    private void SetVisualElementAlpha(VisualElement visualElement, float alpha)
    {
        visualElement.style.opacity = alpha;
    }

    private async Task FadeLabel(Label label, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            label.style.opacity = newAlpha;
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
        label.style.opacity = endAlpha;
    }

}
