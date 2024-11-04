using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// The conversion class that matches the data of the items with their VisualElements.
[Serializable]
public class StoredItem
{
    public Item Details; // The actual data for the item.
    [HideInInspector]
    public Dictionary<InventoryLayout, ItemVisual> RootVisual = new Dictionary<InventoryLayout, ItemVisual>(); // The visual elements mapped to the layout they are in, either the inventory or shop
    [HideInInspector]
    public Rect OverlapRectangle; // The slots this item takes up in the inventory. Top left slot is (0,0) and so on. Used for checking for open slots.
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // Since this class is static you can use this instance to access it following the singleton pattern.

    public List<StoredItem> StoredItems = new List<StoredItem>(); // All the items currently stored in the inventory. Only the Details variables are set in editor, rest are created in LoadInventory.
    public Dimensions InventoryDimensions; // Dimensions of the inventory, set in the editor to a static number based on layout.
    public List<InventoryLayout> InventoryLayouts = new List<InventoryLayout>();

    private bool m_IsInventoryReady; // Bool for signaling that inventory has finished initializing and is ready to load.
    private int m_InventoryValue = 0; // The total value of the inventory, is calculated as items are added and removed.
    [SerializeField]
    private float m_MaxWeight;
    private float m_CurrentWeight;
    [SerializeField]
    private UIDocument m_Hud;
    private Label m_CurrentWeightElement;
    private Label m_MaxWeightElement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Configure();
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartCoroutine(LoadInventory());
    }

    // Initializes the inventory. Should only need to be called in Awake.
    private void Configure()
    {
        m_CurrentWeightElement = m_Hud.rootVisualElement.Q<Label>("CurrentWeight");
        m_MaxWeightElement = m_Hud.rootVisualElement.Q<Label>("MaxWeight");
        m_MaxWeightElement.text = m_MaxWeight.ToString();
        m_CurrentWeightElement.text = m_CurrentWeight.ToString();
        // Makes it so HuD doesn't steal click events, mainly so tooltips will work
        m_Hud.rootVisualElement.Q<VisualElement>("Container").pickingMode = PickingMode.Ignore;
        //m_Root.RegisterCallback<GeometryChangedEvent>(OnLayoutFinished);

        // Give the UI toolkit time to initialize the layout
        //yield return new WaitUntil(() => m_LayoutReady);

        // Mark inventory as ready and initialized
        m_IsInventoryReady = true;
    }




    // Commented out related code for this function as this is no longer needed for now
    // and it seems to not be consistent, as when I switched computers it stopped working.
    // Function that is called when the root element of the inventory document changes.
    // Is used for determining when the layout is finished being created during Configuration.
    // Will be useful to use if we handle changing resolutions.
    /*private void OnLayoutFinished(GeometryChangedEvent evt)
    {
        // Multiple of these are called after registering above, have to wait for the
        // event that matches the current window size otherwise slot size will be off.
        if (Screen.width == evt.newRect.width && Screen.height == evt.newRect.height)
            m_LayoutReady = true;
    }*/

    // Loads the inventory, going through the StoredItems and placing them in the inventory
    // in the first available open slot. Might be useful to call again outside of start if
    // we need to reload the inventory at some point from scratch.
    private IEnumerator LoadInventory()
    {
        // Don't load the inventory until initialization is done
        yield return new WaitUntil(() => m_IsInventoryReady);

        // Create each item in the stored items
        foreach (StoredItem loadedItem in StoredItems)
        {
            CreateItem(loadedItem);
        }
    }

    // Public function for adding an item to the inventory,
    // returns true if there was room, false if there was not
    public bool AddItem(Item item)
    {
        // Create a stored item class for the new item
        StoredItem sItem = new StoredItem
        {
            Details = item
        };

        // Call create item function to create the item in the inventory
        bool result = CreateItem(sItem);

        if(result)
            StoredItems.Add(sItem);

        return result;
    }

    // Creates the visual element for an item and adds it to the inventory grid
    // Only does that if there is room for the item, returning true if there is room and false if there isn't
    private bool CreateItem(StoredItem item)
    {
        if (item.Details.Weight + m_CurrentWeight > m_MaxWeight)
            return false;

        bool inventoryHasSpace = GetPositionForItem(item);

        // If we don't have space remove the item from the grid and continue on
        if (!inventoryHasSpace)
        {
            Debug.LogWarning("No space - Cannot pick up the item");
            item.RootVisual.Clear();
            return false;
        }

        RecalculateWeight();

        return true;
    }

    // Deletes the passed in stored item, getting rid of its icon visual element
    // and removing it from the stored items list
    public void DeleteItem(StoredItem item)
    {
        foreach(KeyValuePair<InventoryLayout, ItemVisual> visual in item.RootVisual)
            // Remove the visual element from it's parent to get rid of it from the document
            visual.Value.parent.Clear();
        // Then remove it from the stored item list
        StoredItems.Remove(item);
        RecalculateWeight();
    }

    // Clears all items from the inventory, removing them from the list and their visual elements
    public void ClearInventory()
    {
        // For each item in the list remove its visual element from it's parent
        foreach (StoredItem item in StoredItems)
            foreach (KeyValuePair<InventoryLayout, ItemVisual> visual in item.RootVisual)
                // Remove the visual element from it's parent to get rid of it from the document
                visual.Value.parent.Clear();
        // Once we finish clearing out the visual elements we can clear the list
        StoredItems.Clear();
        RecalculateWeight();
    }

    // Returns the pre-calculated value of the inventory
    public int GetInventoryvalue()
    {
        RecalculateValue();
        return m_InventoryValue;
    }

    // Finds the first available slot in the inventory and puts the passed in
    // item there. Returns false if there is no available spot.
    private bool GetPositionForItem(StoredItem newItem)
    {
        // Check each position in the inventory to see if it will work for the new item.
        for (int y = 0; y < InventoryDimensions.Height; y++)
        {
            for (int x = 0; x < InventoryDimensions.Width; x++)
            {
                bool free = true;
                // Make a new rectangle for this item based on the current position we are checking.
                Rect itemRect = new Rect(x, y, newItem.Details.SlotDimension.Width, newItem.Details.SlotDimension.Height);

                // If placing the item here would make it outside the inventory continue on from this spot.
                if (x + itemRect.width > InventoryDimensions.Width || y + itemRect.height > InventoryDimensions.Height)
                    continue;

                // Loop through each item to see if it conflicts with placing the item in this spot.
                foreach(StoredItem item in StoredItems)
                {
                    // First check to see if the item has been placed by checking if it has a root visual component.
                    // Second check if this new item overlaps with the item.
                    if (item.RootVisual.Count > 0 && item.OverlapRectangle.Overlaps(itemRect))
                    {
                        free = false;
                        break;
                    }
                }

                // If there was a conflict continue to the next spot.
                if (!free)
                    continue;

                // If no conflict was found this position is valid and will be used for the item.
                // Add a visual for each inventory layout we have, for now this is shop and inventory
                foreach (InventoryLayout layout in InventoryLayouts)
                {
                    ItemVisual newVisual = new ItemVisual(newItem, layout.GetRoot());
                    // Create the new item visual, which is the visual element that appears in the inventory
                    newItem.RootVisual.Add(layout, newVisual);
                    layout.AddItem(newVisual, x, y);
                }
                
                // Add rectangle for future checking for collisions
                newItem.OverlapRectangle = itemRect;

                return true;
            }
        }
        // No space was found for the item
        return false;
    }

    private void RecalculateWeight()
    {
        float weight = 0f;

        foreach(StoredItem item in StoredItems)
        {
            weight += item.Details.Weight;
        }

        m_CurrentWeight = weight;

        m_CurrentWeightElement.text = m_CurrentWeight.ToString();
    }

    private void RecalculateValue()
    {
        int value = 0;

        foreach (StoredItem item in StoredItems)
        {
            value += item.Details.SellPrice;
        }

        m_InventoryValue = value;
    }
}
