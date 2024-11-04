using UnityEngine;
using UnityEngine.UIElements;

public partial class ItemVisual : VisualElement
{
    private StoredItem m_Item;
    private Inventory m_InventoryComp;
    public VisualElement Root;
    private VisualElement m_Inventory;
    public VisualElement Icon;
    private ItemTooltip m_Tooltip;
    private bool m_Dragging = false;

    public ItemVisual(StoredItem item, VisualElement root)
    {
        m_Item = item;
        Root = root;
        m_Inventory = Root.Q<VisualElement>("Inventory");
        m_InventoryComp = Inventory.Instance;

        // Name it based on the user friendly name of the item.
        name = $"{m_Item.Details.FriendlyName}";
        // Set it to hidden at first so it doesn't show up till we find a spot for it in inventory.
        style.visibility = Visibility.Hidden;

        // Create the actual icon image child element
        Icon = new VisualElement
        {
            style = {
                backgroundImage = m_Item.Details.Icon.texture,
                // TODO in the future if we support bigger items change this as this only works with 1x1 items
                // Set the width and length depending on the size of the object using percent as pixel had floating point inaccuracies.
                width = Length.Percent(100 * item.Details.SlotDimension.Width),
                height = Length.Percent(100 * item.Details.SlotDimension.Height)
            }
        };
        Add(Icon);

        // Create the tool tip but don't add it for now, only add it when the mouse hovers over the icon.
        m_Tooltip = new ItemTooltip(m_Item, Root, Icon);

        // Add the selectors to the elements created so the correct styles are applied
        Icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");

        // Register events for pointer to handle tooltip and dragging items.
        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    // When the mouse is pressed on a slot we need to start dragging that item
    void OnPointerDown(PointerDownEvent data)
    {
        // Set the icon's position to the current position of the mouse
        MoveItem(data.localPosition);

        // Register the mouse move event for dragging, use m_Root so it keeps tracking
        // movement even if cursor manages to get outside the icon.
        Root.RegisterCallback<PointerMoveEvent>(DragItem);
        Root.RegisterCallback<PointerUpEvent>(OnPointerUp);

        // Set dragging to true
        m_Dragging = true;
        // Hides the tool tip while we are dragging
        m_Tooltip.StopShowingTooltip();
    }

    void OnPointerUp(PointerUpEvent data) 
    {
        EndDragging(data.position);
    }

    // Mouse move event for dragging the item, called while dragging an item every time the mouse moves
    void DragItem(PointerMoveEvent data)
    {
        // If the mouse is still being held down
        if(Input.GetMouseButton(0))
            // Move the item, converting the world position of the mouse to the local position of the slot
            MoveItem(this.WorldToLocal(data.position));
        // If the mouse isn't held down anymore
        else
            // Call the function to end dragging
            EndDragging(data.position);
    }

    // Ends the dragging, either removing the item if outside the inventory or resetting it if inside the inventory
    void EndDragging(UnityEngine.Vector2 pos)
    {
        // Unregister the drag event
        Root.UnregisterCallback<PointerMoveEvent>(DragItem);
        Root.UnregisterCallback<PointerUpEvent>(OnPointerUp);

        // Check if the mouse is currently inside the inventory box
        if (m_Inventory.localBound.Contains(m_Inventory.WorldToLocal(pos)))
        {
            // If it is reset the item back to its slot
            Icon.style.left = 0;
            Icon.style.top = 0;
        }
        // If it's not inside the inventory
        else
            // TODO add code to spew the item out here
            // Delete the item from the inventory
            m_InventoryComp.DeleteItem(m_Item);

        // Set dragging back to false
        m_Dragging = false;
        // Start showing the tooltip again after being done dragging
        m_Tooltip.StartShowingTooltip();
    }

    // Function for moving the item icons, use this instead of move element for moving an item icon
    void MoveItem(UnityEngine.Vector2 pos)
    {
        MoveElement(Icon, pos);
    }

    // Moves the tooltip based on the provided position, assumed position is in local space of the root element
    void MoveElement(VisualElement item, UnityEngine.Vector2 pos)
    {
        item.style.left = pos.x;
        item.style.top = pos.y;
    }
}
