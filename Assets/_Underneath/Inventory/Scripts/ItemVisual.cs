using System.Numerics;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemVisual : VisualElement
{
    private readonly Item m_Item;
    private VisualElement m_Root;
    private VisualElement m_Icon;
    private VisualElement m_Tooltip;

    public ItemVisual(Item item, VisualElement root)
    {
        m_Item = item;
        m_Root = root;

        // Name it based on the user friendly name of the item.
        name = $"{m_Item.FriendlyName}";
        // Set it to hidden at first so it doesn't show up till we find a spot for it in inventory.
        style.visibility = Visibility.Hidden;

        // Create the actual icon image child element
        m_Icon = new VisualElement
        {
            style = {
                backgroundImage = m_Item.Icon.texture,
                // TODO in the future if we support bigger items change this as this only works with 1x1 items
                // Set the width and length depending on the size of the object using percent as pixel had floating point inaccuracies.
                width = Length.Percent(100 * item.SlotDimension.Width),
                height = Length.Percent(100 * item.SlotDimension.Height)
            }
        };
        Add(m_Icon);

        // Create the tool tip but don't add it for now, only add it when the mouse hovers over the icon.
        m_Tooltip = new VisualElement();

        // Add the selectors to the elements created so the correct styles are applied
        m_Icon.AddToClassList("visual-icon");
        m_Tooltip.AddToClassList("tooltip");
        AddToClassList("visual-icon-container");

        // Register events for pointer entering and leaving the icon.
        RegisterCallback<PointerOverEvent>(OnPointerOver);
        RegisterCallback<PointerOutEvent>(OnPointerOut);
    }

    // When the mouse goes over the inventory slot
    void OnPointerOver(PointerOverEvent data)
    {
        // Convert moust position from world space to local space of root element, then assign that as the position
        MoveTooltip(m_Root.WorldToLocal(data.position));
        // Add the tooltip to the root element so it will render
        m_Root.Add(m_Tooltip);

        // Register the mouse movment event so the tooltip will move with the mouse
        RegisterCallback<PointerMoveEvent>(MouseMovementEvent);
    }

    // When the mouse leaves the inventory slot
    void OnPointerOut(PointerOutEvent data)
    {
        // Deregister the move event as we no longer need it
        UnregisterCallback<PointerMoveEvent>(MouseMovementEvent);

        // Remove the tooltip from the root element so it no longer renders
        m_Root.Remove(m_Tooltip);
    }

    // When the mouse moves, only registered when the mouse is hovering over the inventory slot
    void MouseMovementEvent(PointerMoveEvent data)
    {
        // Convert moust position from world space to local space of root element, then assign that as the position
        MoveTooltip(m_Root.WorldToLocal(data.position));
    }

    // Moves the tooltip based on the provided position, assumed position is in local space of the root element
    void MoveTooltip(UnityEngine.Vector2 pos)
    {
        // Have to have + 1 here so that the tooltip doesn't steal the mouse cursor and cause
        // the pointer out function above to be called too early
        m_Tooltip.style.left = pos.x + 1;
        m_Tooltip.style.top = pos.y + 1;
    }
}
