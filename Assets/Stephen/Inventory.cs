using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// The conversion class that matches the data of the items with their VisualElements.
[Serializable]
public class StoredItem
{
    public InventoryItem Details; // The actual data for the item.
    public ItemVisual RootVisual; // The visual element for displaying the item in the inventory. Icon-Container -> Icon.
    [HideInInspector]
    public Rect OverlapRectangle; // The slots this item takes up in the inventory. Top left slot is (0,0) and so on. Used for checking for open slots.
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // Since this class is static you can use this instance to access it following the singleton pattern.
    public static Vector2 SlotDimension { get; private set; } // Dimensions of a slot, is calculated after layout is ready and is used for positioning Icons.

    public List<StoredItem> StoredItems = new List<StoredItem>(); // All the items currently stored in the inventory. Only the Details variables are set in editor, rest are created in LoadInventory.
    public Dimensions InventoryDimensions; // Dimensions of the inventory, set in the editor to a static number based on layout.

    private Vector2 m_Origin; // Origin position of the inventory based on the first slot in it.
    private VisualElement m_Root; // Root visual element of the inventory, "Container" in the UXML file.
    private VisualElement m_InventoryGrid; // Grid that contains the slot, "Grid" in the UXML file.
    private bool m_IsInventoryReady; // Bool for signaling that inventory has finished initializing and is ready to load.
    private bool m_LayoutReady; // Bool for signaling that the layout engine has finished it's work and VisualElements are setup.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(Configure());
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start() => StartCoroutine(LoadInventory());

    // Initializes the inventory. Should only need to be called in Awake.
    private IEnumerator Configure()
    {
        // Get the document and visual elements we will need
        UIDocument doc = GetComponentInChildren<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_InventoryGrid = m_Root.Q<VisualElement>("Grid");
        m_Root.RegisterCallback<GeometryChangedEvent>(OnLayoutFinished);

        // Give the UI toolkit time to initialize the layout
        yield return new WaitUntil(() => m_LayoutReady);

        // Calculate size of slots
        ConfigureSlotDimensions();
        // Mark inventory as ready and initialized
        m_IsInventoryReady = true;
    }

    // Function that is called when the root element of the inventory document changes.
    // Is used for determining when the layout is finished being created during Configuration.
    // Will be useful to use if we handle changing resolutions.
    private void OnLayoutFinished(GeometryChangedEvent evt)
    {
        // Multiple of these are called after registering above, have to wait for the
        // event that matches the current window size otherwise slot size will be off.
        if (Screen.width == evt.newRect.width && Screen.height == evt.newRect.height)
            m_LayoutReady = true;
    }

    // Loads the inventory, going through the StoredItems and placing them in the inventory
    // in the first available open slot. Might be useful to call again outside of start if
    // we need to reload the inventory at some point from scratch.
    private IEnumerator LoadInventory()
    {
        // Don't load the inventory until initialization is done
        yield return new WaitUntil(() => m_IsInventoryReady);

        foreach (StoredItem loadedItem in StoredItems)
        {
            // Create the new item visual, which is the visual element that appears in the inventory
            loadedItem.RootVisual = new ItemVisual(loadedItem.Details);

            AddItemToInventoryGrid(loadedItem.RootVisual);

            bool inventoryHasSpace = GetPositionForItem(loadedItem);

            // If we don't have space remove the item from the grid and continue on
            // TODO add more here for what happens when inventory is full
            if (!inventoryHasSpace)
            {
                Debug.Log("No space - Cannot pick up the item");
                RemoveItemFromInventoryGrid(loadedItem.RootVisual);
                loadedItem.RootVisual = null;
                continue;
            }

            // Call this to make the item ready to be shown after we know it's in a valid place
            ConfigureInventoryItem(loadedItem);
        }
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
                    if (item.RootVisual != null && item.OverlapRectangle.Overlaps(itemRect))
                    {
                        free = false;
                        break;
                    }
                }

                // If there was a conflict continue to the next spot.
                if (!free)
                    continue;

                // If no conflict was found this position is valid and will be used for the item.
                // Set item position and it's rectangle for overlap checking in the future.
                SetItemPosition(newItem.RootVisual, new Vector2(m_Origin.x + SlotDimension.x * x,
                    m_Origin.y + SlotDimension.y * y));
                newItem.OverlapRectangle = itemRect;

                return true;
            }
        }
        // No space was found for the item
        return false;
    }

    // Sets up the SlotDimension variable with the correct size of slots.
    // Slots afaik shouldn't get bigger or smaller if resolution scales so this shouldn't need to
    // be called again for resolution changes.
    private void ConfigureSlotDimensions()
    {
        // Gets a slot from the grid
        VisualElement firstSlot = m_InventoryGrid.Q<VisualElement>("Slot1");

        // Builds a new vector based on the size of that slot, saves it for later use.
        SlotDimension = new Vector2(firstSlot.worldBound.width, firstSlot.worldBound.height);

        // Save the origin so we can add it to the position as an offset when positioning icons.
        m_Origin = new Vector2(firstSlot.layout.x, firstSlot.layout.y);
    }

    //Sets the position of an element from a vector, using the top left corner as an origin
    private static void SetItemPosition(VisualElement element, Vector2 vector)
    {
        element.style.left = vector.x; // X position is distance from left
        element.style.top = vector.y; // Y position is distance from top
    }

    // Adds the passed in visual element to the inventory grid by adding it as a child of the grid
    private void AddItemToInventoryGrid(VisualElement item) => m_InventoryGrid.Add(item);
    // Removes the passed in visual element from the inventory grid by removing it as a child of the grid
    private void RemoveItemFromInventoryGrid(VisualElement item) => m_InventoryGrid.Remove(item);

    // Configures an item to be ready to be shown in the inventory
    private static void ConfigureInventoryItem(StoredItem item)
    {
        // Since item starts out as invisible when we create it now that's it's confirmed in a good spot make it visible.
        item.RootVisual.style.visibility = Visibility.Visible;
    }
}
