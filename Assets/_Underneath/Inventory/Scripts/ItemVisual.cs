using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ItemVisual : VisualElement
{
    private readonly Item m_Item;
    private VisualElement m_Root;
    private VisualElement m_Icon;
    private VisualElement m_Tooltip;
    private bool m_TooltipShown = false;
    private bool m_Dragging = false;

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

        // Register events for pointer to handle tooltip and dragging items.
        RegisterCallback<PointerOverEvent>(OnPointerOver);
        RegisterCallback<PointerOutEvent>(OnPointerOut);
        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    // When the mouse is pressed on a slot we need to start dragging that item
    void OnPointerDown(PointerDownEvent data)
    {
        // Hides the tool tip while we are dragging
        HideTooltip();

        // Set the icon's position to the current position of the mouse
        MoveItem(data.localPosition);

        // Register the mouse move event for dragging, use m_Root so it keeps tracking
        // movement even if cursor manages to get outside the icon.
        m_Root.RegisterCallback<PointerMoveEvent>(DragItem);

        // Set dragging to true
        m_Dragging = true;
    }

    // Mouse move event for dragging the item, called while dragging an item every time the mouse moves
    void DragItem(PointerMoveEvent data)
    {
        // If the player is still holding the mouse button and dragging the item
        if (Input.GetMouseButton(0))
            // Move the item, converting the world position of the mouse to the local position of the slot
            MoveItem(this.WorldToLocal(data.position));
        // If we're not holding the mouse anymore we need to stop dragging
        else
        {
            // Unregister the drag event
            m_Root.UnregisterCallback<PointerMoveEvent>(DragItem);

            // For now reset the icon back to its origin, this will be for invalid drags
            m_Icon.style.left = 0;
            m_Icon.style.top = 0;

            // Set dragging back to false
            m_Dragging = false;
        }
    }

    // When the mouse goes over the inventory slot start showing the tooltip
    void OnPointerOver(PointerOverEvent data)
    {
        ShowTooltip(data.position);
    }

    // When the mouse leaves the inventory slot hide the tooltip
    void OnPointerOut(PointerOutEvent data)
    {
        HideTooltip();
    }

    // Shows the tooltip and handles registering the mouse movement event for moving it with the mouse
    void ShowTooltip(UnityEngine.Vector2 pos)
    {
        // Check to make sure we aren't already showing the tooltip and that we aren't dragging the item
        if (m_TooltipShown == false && m_Dragging == false)
        {
            // Convert moust position from world space to local space of root element, then assign that as the position
            MoveTooltip(m_Root.WorldToLocal(pos));
            // Add the tooltip to the root element so it will render
            m_Root.Add(m_Tooltip);

            // Register the mouse movment event so the tooltip will move with the mouse
            RegisterCallback<PointerMoveEvent>(TooltipMouseMovement);

            // Set tooltip to shown locally
            m_TooltipShown = true;
        }
    }

    // Function that actually removes the tooltip and gets rid of the mouse movement callback event
    void HideTooltip()
    {
        // Check to make sure the tool tip is actually be shown
        if (m_TooltipShown == true)
        {
            // Deregister the move event as we no longer need it since tooltip will be hidden
            UnregisterCallback<PointerMoveEvent>(TooltipMouseMovement);

            // Remove the tooltip from the root element so it no longer renders
            m_Root.Remove(m_Tooltip);

            // Set tooltip to hidden locally
            m_TooltipShown = false;
        }
    }

    // When the mouse moves this is called to move the tooltip, only registered when the mouse 
    // is hovering over the inventory slot and when the player isn't dragging an item.
    void TooltipMouseMovement(PointerMoveEvent data)
    {
        // Convert moust position from world space to local space of root element, then assign that as the position
        MoveTooltip(m_Root.WorldToLocal(data.position));
    }

    // Function for moving the tooltip to the passed position, use this instead of move element for moving tooltip
    void MoveTooltip(UnityEngine.Vector2 pos)
    {
        // Have to have + 1 here so that the tooltip doesn't steal the mouse cursor and cause
        // the pointer out function above to be called too early
        MoveElement(m_Tooltip, pos + new UnityEngine.Vector2(1, 1));
    }

    // Function for moving the item icons, use this instead of move element for moving an item icon
    void MoveItem(UnityEngine.Vector2 pos)
    {
        MoveElement(m_Icon, pos);
    }

    // Moves the tooltip based on the provided position, assumed position is in local space of the root element
    void MoveElement(VisualElement item, UnityEngine.Vector2 pos)
    {
        item.style.left = pos.x;
        item.style.top = pos.y;
    }
}
