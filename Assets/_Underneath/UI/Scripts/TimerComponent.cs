using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TimerComponent : MonoBehaviour
{
    float m_Time = 0f;
    float m_TimeLimit = 1f;
    VisualElement m_ClockHand;

    public float TimeRemaining
    {
        get => m_Time;
        set
        {
            m_Time = Mathf.Clamp(value, 0f, m_TimeLimit);
            UpdateClock();
        }
    }

    public float TimeLimit
    {
        get => m_TimeLimit;
        set
        {
            m_TimeLimit = value;
            UpdateClock();
        }
    }

    private void Start()
    {
        m_ClockHand = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("ClockHand");
    }

    void UpdateClock()
    {
        float rot = ((m_TimeLimit - m_Time) / m_TimeLimit) * 360f;

        m_ClockHand.style.rotate = new Rotate(rot);
    }
}
