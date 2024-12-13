using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OpeningCutscene : OurMonoBehaviour
{
    [Header("Define Me!")]
    [SerializeField] private float CharactersPerSecond;
    [SerializeField] private float normalDelay;
    [SerializeField] private float extraDelay;
    [SerializeField] private float longDelay;
    [SerializeField] private float musicDelay;

    [SerializeField] private float fadeDuration;
    
    private UIDocument m_Document;
    private VisualElement m_Root;
    private Label m_Text1;
    private Label m_Text2;
    private Label m_Text3;
    private VisualElement m_Image;
    
    private void Start()
    {
        if (CharactersPerSecond == 0 || normalDelay == 0 || extraDelay == 0 || longDelay == 0 || fadeDuration == 0)
        {
            Debug.LogException(new Exception($"One Of The Variables Has Not Been Assigned In The Inspector!"));
        }
        
        m_Document = GetComponent<UIDocument>();
        m_Root = m_Document.rootVisualElement;
        
        m_Text1 = m_Root.Q<Label>("FirstLine");
        SetLabelAlpha(m_Text1, 0f);
        m_Text2 = m_Root.Q<Label>("SecondLine");
        
        SetLabelAlpha(m_Text2, 0f);
        m_Text3 = m_Root.Q<Label>("LastLine");
        SetLabelAlpha(m_Text3, 0f);
        
        m_Image = m_Root.Q<VisualElement>("Photograph");
        SetVisualElementAlpha(m_Image,0f);
        
        RunCutscene();
    }

    public async Task RunCutscene()
    {
        await Task.Delay(TimeSpan.FromSeconds(musicDelay));
        
        await StartSlide(normalDelay, extraDelay, longDelay, String.Empty,". . .",String.Empty);
        
        await Task.Delay(TimeSpan.FromSeconds(longDelay));
        
        await StartSlide(normalDelay, extraDelay, longDelay, 
            "To Luca, Rowan, and Asher", 
            "How is everything at Mrs. Walker’s house?", 
            "I hope you’ve made some friends with other kids there");
        
        await Task.Delay(TimeSpan.FromSeconds(longDelay));

        await StartSlide(normalDelay, extraDelay, longDelay, 
            "I can only imagine how big you’re getting", 
            "and I know you have a lot of questions", 
            "and I’ll answer all of them when I’m back.");
        
        await Task.Delay(TimeSpan.FromSeconds(longDelay));

        await StartSlide(normalDelay, extraDelay, longDelay, 
            "Sometimes in life, we have to make hard decisions in order to do what’s right", 
            "and nothing was harder for me then having to leave the three of you.");
        
        await Task.Delay(TimeSpan.FromSeconds(longDelay));

        await StartSlide(normalDelay, extraDelay, longDelay, 
            "I hope you understand that this is what’s best for our family", 
            "For you three", "I need you to be brave for just a little bit longer.");
        
        await Task.Delay(TimeSpan.FromSeconds(longDelay));

        await StartSlide(normalDelay, extraDelay, longDelay, 
            "I’ll see you soon, my loves", String.Empty, "-Mom", true);
        
       // await Task.Delay(TimeSpan.FromSeconds(longDelay));
        
        LoadMainMenu();
    }

    private async Task LoadMainMenu()
    {
        var asyncLoad  = SceneManager.LoadSceneAsync("MainMenu",LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }
        
        SceneManager.UnloadSceneAsync(1);
    }

    private async Task StartSlide(float delay1Line, float delay2Line, float delay3Line, string line1 = null, string line2 = null, string line3 = null, bool photo = false)
    {
        if (photo)
        {
            m_Text2.style.display = DisplayStyle.None;
            m_Image.style.display = DisplayStyle.Flex;
            FadeVisualElement(m_Image,0f,1f,1f);
        }
        
        await DisplayLines(line1, line2, line3, delay1Line, delay2Line, delay3Line);
    }

    private async Task DisplayLines(string line1, string line2, string line3, float delay1, float delay2, float delay3)
    {
        if (!string.IsNullOrEmpty(line1))
        {
            await TypeText(line1, m_Text1);
            await Task.Delay(TimeSpan.FromSeconds(delay1));
        }

        if (!string.IsNullOrEmpty(line2))
        {
            await TypeText(line2, m_Text2);
            await Task.Delay(TimeSpan.FromSeconds(delay2));
        }

        if (!string.IsNullOrEmpty(line3))
        {
            await TypeText(line3, m_Text3);
            await Task.Delay(TimeSpan.FromSeconds(delay3));
        }

        List<Label> labels = new()
        {
            m_Text1,
            m_Text2,
            m_Text3
        };

        
        await FadeLabelsAsync(labels, 1f, 0f, fadeDuration);
    }

    private async Task TypeText(string line, Label label)
    {
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
    
    private void SetLabelAlpha(Label label, float alpha)
    {
        label.style.opacity = alpha;
    }
    
    private void SetVisualElementAlpha(VisualElement visualElement, float alpha)
    {
        visualElement.style.opacity = alpha;
    }
    
    private void FadeVisualElement(VisualElement visualElement, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            visualElement.style.opacity = newAlpha;
            elapsedTime += Time.deltaTime;
        }
        visualElement.style.opacity = endAlpha;
    }
    
    
    private async Task FadeLabelAsync(Label label, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            label.style.opacity = newAlpha;
            elapsedTime += Time.deltaTime;
            await Task.Delay(TimeSpan.FromSeconds(0.1f)); 
        }
        label.style.opacity = endAlpha; 
    }
    
    private async Task FadeLabelsAsync(List<Label> labels, float startAlpha, float endAlpha, float duration, bool photo = false)
    {
        var fadeTasks = new List<Task>();

        foreach (Label label in labels)
        {
            if (label.style.opacity == 0f)
                continue;

            fadeTasks.Add(FadeLabelAsync(label, startAlpha, endAlpha, duration));
        }
        
        if (photo)
        {
            fadeTasks.Add(FadeVisualElementAsync(m_Image,startAlpha,endAlpha,duration));
        }
        
        await Task.WhenAll(fadeTasks);
    }

    private async Task FadeVisualElementAsync(VisualElement visualElement, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            visualElement.style.opacity = newAlpha;
            elapsedTime += Time.deltaTime;
            await Task.Delay(0);
        }
        visualElement.style.opacity = endAlpha;
    }
}
