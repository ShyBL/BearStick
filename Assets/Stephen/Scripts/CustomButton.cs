using UnityEngine;
using UnityEngine.UIElements;

public partial class CustomButton : Button
{
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

        UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
        {
            name = "Text",
            defaultValue = "Test"
        };

        Button m_Button;
        float m_MinRandomRotation;
        float m_MaxRandomRotation;
        float m_MinHoverRotation;
        float m_MaxHoverRotation;
        int m_Direction;


        public override void Init(VisualElement button, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(button, bag, cc);
            Button but = button as Button;
            but.RegisterCallback<MouseEnterEvent>(OnHover);
            but.RegisterCallback<MouseLeaveEvent>(EndHover);
            but.text = m_Text.GetValueFromBag(bag, cc);

            m_Button = but;
            m_MinRandomRotation = m_MinRandomRotationAtt.GetValueFromBag(bag, cc);
            m_MaxRandomRotation = m_MaxRandomRotationAtt.GetValueFromBag(bag, cc);
            m_MinHoverRotation = m_MinHoverRotationAtt.GetValueFromBag(bag, cc);
            m_MaxHoverRotation = m_MaxHoverRotationAtt.GetValueFromBag(bag, cc);
            if (Random.Range(0f, 2f) >= 1f)
                m_Direction = -1;
            else
                m_Direction = 1;

            EndHover();
        }

        void OnHover(MouseEnterEvent e)
        {
            float angle = Random.Range(m_MinHoverRotation, m_MaxHoverRotation) * m_Direction;

            m_Direction *= -1;

            m_Button.style.rotate = new Rotate(angle);
        }

        void EndHover(MouseLeaveEvent e = null)
        {
            float angle = Random.Range(m_MinRandomRotation, m_MaxRandomRotation) * m_Direction;

            m_Button.style.rotate = new Rotate(angle);
        }
    }
}
