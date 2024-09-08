using UnityEngine;
using UnityEngine.UIElements;

public partial class CustomButton : Button
{
    // These need getters and setters and have to be named the same as the
    // name property in the UxmlTraits otherwise serialization doesnt work.
    float MinRandomRotation { get; set; }
    float MaxRandomRotation { get; set; }
    float MinHoverRotation { get; set; }
    float MaxHoverRotation { get; set; }
    int StartingDirection { get; set; }

    public CustomButton()
    {
        RegisterCallback<MouseEnterEvent>(OnHover);
        RegisterCallback<MouseLeaveEvent>(EndHover);
    }

    public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }
    public new class UxmlTraits : BindableElement.UxmlTraits
    {
        UxmlFloatAttributeDescription m_MinRandomRotationAttr = new UxmlFloatAttributeDescription
        {
            name = "Min-Random-Rotation",
            defaultValue = 4.0f
        };
        UxmlFloatAttributeDescription m_MaxRandomRotationAttr = new UxmlFloatAttributeDescription
        {
            name = "Max-Random-Rotation",
            defaultValue = 6.0f
        };
        UxmlFloatAttributeDescription m_MinHoverRotationAttr = new UxmlFloatAttributeDescription
        {
            name = "Min-Hover-Rotation",
            defaultValue = 0.0f
        };
        UxmlFloatAttributeDescription m_MaxHoverRotationAttr = new UxmlFloatAttributeDescription
        {
            name = "Max-Hover-Rotation",
            defaultValue = 0.0f
        };
        UxmlIntAttributeDescription m_StartingDirAttr = new UxmlIntAttributeDescription
        {
            name = "Starting-Direction",
            defaultValue = 1
        };
        UxmlStringAttributeDescription m_TextAttr = new UxmlStringAttributeDescription
        {
            name = "Text",
            defaultValue = "Test"
        };

        public override void Init(VisualElement button, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(button, bag, cc);
            CustomButton but = button as CustomButton;
            but.text = m_TextAttr.GetValueFromBag(bag, cc);

            but.MinRandomRotation = m_MinRandomRotationAttr.GetValueFromBag(bag, cc);
            but.MaxRandomRotation = m_MaxRandomRotationAttr.GetValueFromBag(bag, cc);
            but.MinHoverRotation = m_MinHoverRotationAttr.GetValueFromBag(bag, cc);
            but.MaxHoverRotation = m_MaxHoverRotationAttr.GetValueFromBag(bag, cc);
            but.StartingDirection = m_StartingDirAttr.GetValueFromBag(bag, cc);

            but.EndHover();
        }
    }

    void OnHover(MouseEnterEvent e)
    {
        float angle = Random.Range(MinHoverRotation, MaxHoverRotation) * StartingDirection;

        StartingDirection *= -1;

        style.rotate = new Rotate(angle);
    }

    void EndHover(MouseLeaveEvent e = null)
    {
        float angle = Random.Range(MinRandomRotation, MaxRandomRotation) * StartingDirection;

        style.rotate = new Rotate(angle);
    }
}
