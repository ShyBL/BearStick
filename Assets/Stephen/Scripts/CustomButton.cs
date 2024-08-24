using UnityEngine;
using UnityEngine.UIElements;

public partial class CustomButton : Button
{
    float m_MinRandomRotation;
    float m_MaxRandomRotation;
    float m_MinHoverRotation;
    float m_MaxHoverRotation;
    int m_Direction;

    public CustomButton()
    {
        RegisterCallback<MouseEnterEvent>(OnHover);
        RegisterCallback<MouseLeaveEvent>(EndHover);
    }

    public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }
    public new class UxmlTraits : BindableElement.UxmlTraits
    {
        UxmlFloatAttributeDescription m_MinRandomRotationAtt = new UxmlFloatAttributeDescription
        {
            name = "Min-Random-Rotation",
            defaultValue = 4.0f
        };
        UxmlFloatAttributeDescription m_MaxRandomRotationAtt = new UxmlFloatAttributeDescription
        {
            name = "Min-Random-Rotation",
            defaultValue = 6.0f
        };
        UxmlFloatAttributeDescription m_MinHoverRotationAtt = new UxmlFloatAttributeDescription
        {
            name = "Min-Random-Rotation",
            defaultValue = 0.0f
        };
        UxmlFloatAttributeDescription m_MaxHoverRotationAtt = new UxmlFloatAttributeDescription
        {
            name = "Min-Random-Rotation",
            defaultValue = 0.0f
        };
        UxmlIntAttributeDescription m_StartingDir = new UxmlIntAttributeDescription
        {
            name = "Starting-Direction",
            defaultValue = 1
        };
        UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
        {
            name = "Text",
            defaultValue = "Test"
        };

        public override void Init(VisualElement button, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(button, bag, cc);
            CustomButton but = button as CustomButton;
            but.text = m_Text.GetValueFromBag(bag, cc);

            but.m_MinRandomRotation = m_MinRandomRotationAtt.GetValueFromBag(bag, cc);
            but.m_MaxRandomRotation = m_MaxRandomRotationAtt.GetValueFromBag(bag, cc);
            but.m_MinHoverRotation = m_MinHoverRotationAtt.GetValueFromBag(bag, cc);
            but.m_MaxHoverRotation = m_MaxHoverRotationAtt.GetValueFromBag(bag, cc);
            but.m_Direction = m_StartingDir.GetValueFromBag(bag, cc);

            but.EndHover();
        }
    }

    void OnHover(MouseEnterEvent e)
    {
        float angle = Random.Range(m_MinHoverRotation, m_MaxHoverRotation) * m_Direction;

        m_Direction *= -1;

        style.rotate = new Rotate(angle);
    }

    void EndHover(MouseLeaveEvent e = null)
    {
        float angle = Random.Range(m_MinRandomRotation, m_MaxRandomRotation) * m_Direction;

        style.rotate = new Rotate(angle);
    }
}
