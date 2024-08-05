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
    public ItemVisual RootVisual; // The visual element for displaying the item in the inventory. Icon-Container -> Icon.
    [HideInInspector]
    public Rect OverlapRectangle; // The slots this item takes up in the inventory. Top left slot is (0,0) and so on. Used for checking for open slots.
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // Since this class is static you can use this instance to access it following the singleton pattern.

    public List<StoredItem> StoredItems = new List<StoredItem>(); // All the items currently stored in the inventory. Only the Details variables are set in editor, rest are created in LoadInventory.
    public Dimensions InventoryDimensions; // Dimensions of the inventory, set in the editor to a static number based on layout.

    private List<List<VisualElement>> m_SlotMap = new List<List<VisualElement>>();
    private VisualElement m_Root; // Root visual element of the inventory, all elements in the UXML file are children of this.
    private VisualElement m_InventoryGrid; // Grid that contains the slot, "Grid" in the UXML file.
    private bool m_IsInventoryReady; // Bool for signaling that inventory has finished initializing and is ready to load.
    private bool m_LayoutReady; // Bool for signaling that the layout engine has finished it's work and VisualElements are setup.
    private int m_InventoryValue = 0; // The total value of the inventory, is calculated as items are added and removed.
    [SerializeField]
    private float m_MaxWeight;
    private float m_CurrentWeight;
    [SerializeField]
    private UIDocument m_Hud;
    private Label m_CurrentWeightElement;
    private Label m_MaxWeightElement;
    private Button m_InventoryButton;

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

    private void OnDisable()
    {
        Player.Instance.playerInput.onBagOpened -= onToggleInventory;
    }

    private void OnEnable()
    {
       
    }

    private void Start()
    {
        Player.Instance.playerInput.onBagOpened += onToggleInventory;
        StartCoroutine(LoadInventory());
    }

    // Initializes the inventory. Should only need to be called in Awake.
    private void Configure()
    {
        // Get the document and visual elements we will need
        UIDocument doc = GetComponentInChildren<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_InventoryGrid = m_Root.Q<VisualElement>("Grid");
        m_CurrentWeightElement = m_Hud.rootVisualElement.Q<Label>("CurrentWeight");
        m_MaxWeightElement = m_Hud.rootVisualElement.Q<Label>("MaxWeight");
        m_MaxWeightElement.text = m_MaxWeight.ToString();
        m_CurrentWeightElement.text = m_CurrentWeight.ToString();
        m_InventoryButton = m_Hud.rootVisualElement.Q<Button>("Inventory");
        m_InventoryButton.RegisterCallback<ClickEvent>(ToggleInventory);
        //m_Root.RegisterCallback<GeometryChangedEvent>(OnLayoutFinished);

        // Give the UI toolkit time to initialize the layout
        //yield return new WaitUntil(() => m_LayoutReady);

        // Create a map of all the slots
        ConfigureSlotMap();
        // Mark inventory as ready and initialized
        m_IsInventoryReady = true;
    }


    private void ToggleInventory(ClickEvent evt = null)
    {
      
        AudioManager.instance.PlayOneShot(FMODEvents.instance.OpenBag, Player.Instance.gameObject.transform.position);
       
        switch (m_Root.resolvedStyle.display)
        {
            case DisplayStyle.Flex:
                m_Root.style.display = DisplayStyle.None;
                break;
            case DisplayStyle.None:
                m_Root.style.display = DisplayStyle.Flex;
                break;
        }
    }

    private void onToggleInventory()
    {
        ToggleInventory();
    }

    // Commented out related code for this function as this is no longer needed for now
    // and it seems to not be consistent, as when I switched computers it stopped working.
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

        // Create each item in the stored items
        foreach (StoredItem loadedItem in StoredItems)
        {
            CreateItem(loadedItem);
        }

        // Now that inventory is setup time to hide it by default.
        m_Root.style.display = DisplayStyle.None;
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

        // Create the new item visual, which is the visual element that appears in the inventory
        item.RootVisual = new ItemVisual(item, m_Root, this);

        AddItemToInventoryGrid(item.RootVisual);

        bool inventoryHasSpace = GetPositionForItem(item);

        // If we don't have space remove the item from the grid and continue on
        if (!inventoryHasSpace)
        {
            Debug.Log("No space - Cannot pick up the item");
            RemoveItemFromInventoryGrid(item.RootVisual);
            item.RootVisual = null;
            return false;
        }

        // Call this to make the item ready to be shown after we know it's in a valid place
        ConfigureInventoryItem(item);

        // Add to the invevntory value the value of the item that was just added
        m_InventoryValue += item.Details.SellPrice;
        m_CurrentWeight += item.Details.Weight;
        m_CurrentWeightElement.text = m_CurrentWeight.ToString();

        return true;
    }

    // Deletes the passed in stored item, getting rid of its icon visual element
    // and removing it from the stored items list
    public void DeleteItem(StoredItem item)
    {
        // Remove the visual element from it's parent to get rid of it from the document
        item.RootVisual.parent.Clear();
        // Then remove it from the stored item list
        StoredItems.Remove(item);
        // Remove from the inventory value the price of the item that was just removed
        m_InventoryValue -= item.Details.SellPrice;
        m_CurrentWeight -= item.Details.Weight;
        m_CurrentWeightElement.text = m_CurrentWeight.ToString();
    }

    // Clears all items from the inventory, removing them from the list and their visual elements
    public void ClearInventory()
    {
        // For each item in the list remove its visual element from it's parent
        foreach (StoredItem item in StoredItems)
            item.RootVisual.parent.Clear();
        // Once we finish clearing out the visual elements we can clear the list
        StoredItems.Clear();
        // Reset the inventory value
        m_InventoryValue = 0;
        m_CurrentWeight = 0;
        m_CurrentWeightElement.text = m_CurrentWeight.ToString();
    }

    // Returns the pre-calculated value of the inventory
    public int GetInventoryvalue()
    {
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
                // Add item as child of the correct slot and it's rectangle for overlap checking in the future.
                m_SlotMap[y][x].Add(newItem.RootVisual);
                newItem.OverlapRectangle = itemRect;

                return true;
            }
        }
        // No space was found for the item
        return false;
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
            if(x >= InventoryDimensions.Width)
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
