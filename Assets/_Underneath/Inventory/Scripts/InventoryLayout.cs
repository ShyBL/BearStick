using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryLayout : MonoBehaviour
{
    private List<List<VisualElement>> m_SlotMap = new List<List<VisualElement>>();
    private VisualElement m_Root; // Root visual element of the inventory, all elements in the UXML file are children of this.
    private VisualElement m_InventoryGrid; // Grid that contains the slot, "Grid" in the UXML file.
    private WalletTooltip m_WalletTip;

    // Start is called before the first frame update
    void Start()
    {
        // Get the document and visual elements we will need
        UIDocument doc = GetComponentInChildren<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_InventoryGrid = m_Root.Q<VisualElement>("Grid");
        m_WalletTip = new WalletTooltip(m_Root, m_Root.Q<VisualElement>("Wallet"));

        // Create a map of all the slots
        ConfigureSlotMap();
    }

    public VisualElement GetRoot()
    {
        return m_Root;
    }

    public void AddItem(ItemVisual item, int x, int y)
    {
        m_SlotMap[y][x].Add(item);

        item.style.visibility = Visibility.Visible;
    }

    // Gets all slots from the grid and adds them to a 2D list to map all the slots
    private void ConfigureSlotMap()
    {
        // For keeping track of the current coordinate we're on in the loop
        int x = 0; int y = 0;

        // Start by adding the first list to the 2d list
        m_SlotMap.Add(new List<VisualElement>());

        // Slots are already in order due to how they were created in the UI builder
        foreach (VisualElement slot in m_InventoryGrid.Children())
        {
            // If we go over the edge of the inventory width wise
            if (x >= Inventory.Instance.InventoryDimensions.Width)
            {
                // Add a new row
                m_SlotMap.Add(new List<VisualElement>());
                // Wrap around to the first slot on the next row
                x = 0;
                y++;
            }

            // Add the slot visual element to it's position in the 2d list
            m_SlotMap[y].Add(slot);

            // Move on to the next slot
            x++;
        }
    }
}
