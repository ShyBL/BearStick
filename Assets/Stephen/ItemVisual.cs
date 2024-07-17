using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemVisual : VisualElement
{
    private readonly InventoryItem m_Item;

    public ItemVisual(InventoryItem item)
    {
        m_Item = item;

        name = $"{m_Item.FriendlyName}";
        style.height = m_Item.SlotDimension.Height *
            Inventory.SlotDimension.y;
        style.width = m_Item.SlotDimension.Width *
            Inventory.SlotDimension.x;
        style.visibility = Visibility.Hidden;

        VisualElement icon = new VisualElement
        {
            style = { backgroundImage = m_Item.Icon.texture }
        };
        Add(icon);

        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");
    }

    public void SetPosition(Vector2 pos)
    {
        style.left = pos.x;
        style.top = pos.y;
    }
}
