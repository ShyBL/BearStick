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
    private Vector2 m_OriginalPosition;
    private VisualElement m_DragPreview;

    public ItemVisual(StoredItem item, VisualElement root)
    {
        m_Item = item;
        Root = root;
        m_Inventory = Root.Q<VisualElement>("Inventory");
        m_InventoryComp = Inventory.Instance;

        name = $"{m_Item.Details.FriendlyName}";
        style.visibility = Visibility.Hidden;

        Icon = new VisualElement
        {
            focusable = true,
            style = {
                backgroundImage = m_Item.Details.Icon.texture,
                width = Length.Percent(100 * item.Details.SlotDimension.Width),
                height = Length.Percent(100 * item.Details.SlotDimension.Height)
            }
        };
        Add(Icon);

        m_Tooltip = new ItemTooltip(m_Item, Root, Icon);

        Icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");

        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    void OnPointerDown(PointerDownEvent data)
    {
        StartDragging(data.localPosition);
        Root.RegisterCallback<PointerMoveEvent>(DragItem);
        Root.RegisterCallback<PointerUpEvent>(OnPointerUp);
        m_Tooltip.StopShowingTooltip();
    }

    void StartDragging(Vector2 startPos)
    {
        m_Dragging = true;
        m_OriginalPosition = new Vector2(Icon.style.left.value.value, Icon.style.top.value.value);
        
        // Create drag preview that renders on top
        m_DragPreview = new VisualElement
        {
            style = {
                position = Position.Absolute,
                backgroundImage = m_Item.Details.Icon.texture,
                width = Icon.resolvedStyle.width,
                height = Icon.resolvedStyle.height,
                opacity = 0.8f
            }
        };
        m_DragPreview.AddToClassList("visual-icon");
        m_DragPreview.AddToClassList("drag-preview");
        Root.Add(m_DragPreview);
        
        // Hide original icon during drag
        Icon.style.opacity = 0.3f;
        
        MoveElement(m_DragPreview, startPos);
    }

    void OnPointerUp(PointerUpEvent data) 
    {
        EndDragging(data.position);
    }

    void DragItem(PointerMoveEvent data)
    {
        if (Input.GetMouseButton(0) && m_DragPreview != null)
            MoveElement(m_DragPreview, Root.WorldToLocal(data.position));
        else
            EndDragging(data.position);
    }

    void EndDragging(Vector2 pos)
    {

        var grid = Root.Q<VisualElement>("Grid");
        var lPos = grid.WorldToLocal(pos);
        var normalizedPos = new Vector2(lPos.x / grid.resolvedStyle.width, lPos.y / grid.resolvedStyle.height);
        Debug.Log($"Ending drag at {lPos}");

        Root.UnregisterCallback<PointerMoveEvent>(DragItem);
        Root.UnregisterCallback<PointerUpEvent>(OnPointerUp);

        // Clean up drag preview
        if (m_DragPreview != null)
        {
            Root.Remove(m_DragPreview);
            m_DragPreview = null;
        }
        
        Icon.style.opacity = 1f;

        if (m_Inventory.localBound.Contains(m_Inventory.WorldToLocal(pos)))
        {
            Vector2 localPos = m_Inventory.WorldToLocal(pos);
            Vector2Int gridPos = GetGridPosition(localPos);
            
            if (CanPlaceItemAt(gridPos.x, gridPos.y))
            {
                MoveItemToPosition(gridPos.x, gridPos.y);
            }
            else
            {
                // Reset to original position if can't place
                Icon.style.left = m_OriginalPosition.x;
                Icon.style.top = m_OriginalPosition.y;
            }
        }
        else
        {
            // Drop outside inventory - remove item
            m_InventoryComp.DeleteItem(m_Item);
        }

        m_Dragging = false;
        m_Tooltip.StartShowingTooltip();
    }

    bool CanPlaceItemAt(int x, int y)
    {
        Rect itemRect = new Rect(x, y, m_Item.Details.SlotDimension.Width, m_Item.Details.SlotDimension.Height);
        
        // Check bounds
        if (x + itemRect.width > m_InventoryComp.InventoryDimensions.Width || 
            y + itemRect.height > m_InventoryComp.InventoryDimensions.Height)
            return false;

        // Check for overlaps with other items (excluding this item)
        foreach(StoredItem item in m_InventoryComp.StoredItems)
        {
            if (item != m_Item && item.RootVisual.Count > 0 && item.OverlapRectangle.Overlaps(itemRect))
                return false;
        }
        
        return true;
    }

    void MoveItemToPosition(int x, int y)
    {
        // Remove from current position
        foreach(var layoutVisualPair in m_Item.RootVisual)
        {
            layoutVisualPair.Value.RemoveFromHierarchy();
        }
        
        // Update the item's overlap rectangle
        m_Item.OverlapRectangle = new Rect(x, y, m_Item.Details.SlotDimension.Width, m_Item.Details.SlotDimension.Height);
        
        // Add to new position in all layouts
        foreach(var layoutVisualPair in m_Item.RootVisual)
        {
            layoutVisualPair.Key.AddItem(layoutVisualPair.Value, x, y);
        }
        
        // Reset local position
        Icon.style.left = 0;
        Icon.style.top = 0;
    }

    Vector2Int GetGridPosition(Vector2 localPos)
    {
        // Estimate slot size based on inventory dimensions and container size
        float containerWidth = m_Inventory.resolvedStyle.width;
        float containerHeight = m_Inventory.resolvedStyle.height;
        
        float slotWidth = containerWidth / m_InventoryComp.InventoryDimensions.Width;
        float slotHeight = containerHeight / m_InventoryComp.InventoryDimensions.Height;
        
        return new Vector2Int(
            Mathf.FloorToInt(localPos.x / slotWidth),
            Mathf.FloorToInt(localPos.y / slotHeight)
        );
    }

    void MoveItem(Vector2 pos)
    {
        MoveElement(Icon, pos);
    }

    void MoveElement(VisualElement item, Vector2 pos)
    {
        item.style.left = pos.x;
        item.style.top = pos.y;
    }
}
