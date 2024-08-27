using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public abstract class Tooltip : VisualElement
{
    private VisualElement m_Root;
    private VisualElement m_HoverElement;
    private bool m_TooltipShown = false;
    private bool m_Hidden = false;

    public Tooltip(VisualElement root, VisualElement hover)
    {
        m_Root = root;
        m_HoverElement = hover;

        m_HoverElement.RegisterCallback<PointerOverEvent>(OnPointerOver);
        m_HoverElement.RegisterCallback<PointerOutEvent>(OnPointerOut);
    }

    public void StopShowingTooltip()
    {
        m_Hidden = true;
    }

    public void StartShowingTooltip()
    {
        m_Hidden = false;
    }

    protected abstract void UpdateData();

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
        if (m_TooltipShown == false && m_Hidden == false)
        {
            // Convert moust position from world space to local space of root element, then assign that as the position
            MoveTooltip(m_Root.WorldToLocal(pos));
            // Add the tooltip to the root element so it will render
            m_Root.Add(this);

            // Add item information here so that if it changes while the game is running it will update
            UpdateData();

            // Register the mouse movment event so the tooltip will move with the mouse
            m_HoverElement.RegisterCallback<PointerMoveEvent>(TooltipMouseMovement);

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
            m_HoverElement.UnregisterCallback<PointerMoveEvent>(TooltipMouseMovement);

            // Remove the tooltip from the root element so it no longer renders
            m_Root.Remove(this);

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
        MoveElement(this, pos + new UnityEngine.Vector2(1, 1));
    }

    void MoveElement(VisualElement item, UnityEngine.Vector2 pos)
    {
        item.style.left = pos.x;
        item.style.top = pos.y;
    }
}

public class ItemTooltip : Tooltip
{
    private Label m_TooltipTitle;
    private Label m_TooltipWeight;
    private Label m_TooltipValue;
    private Label m_TooltipDesc;
    private StoredItem m_Item;

    public ItemTooltip(StoredItem item, VisualElement root, VisualElement hover) : base(root, hover)
    {
        m_Item = item;

        m_TooltipTitle = new Label("Title");
        m_TooltipValue = new Label("Value");
        m_TooltipWeight = new Label("Weight");
        m_TooltipDesc = new Label("Description");
        VisualElement bar = new VisualElement();

        // Add all the sub parts of the tool tip component to the tooltip
        Add(m_TooltipTitle);
        Add(bar);
        bar.Add(m_TooltipWeight);
        bar.Add(m_TooltipValue);
        Add(m_TooltipDesc);

        bar.AddToClassList("tooltip-horizontal");
        m_TooltipTitle.AddToClassList("tooltip-title");
        m_TooltipValue.AddToClassList("tooltip-text");
        m_TooltipWeight.AddToClassList("tooltip-text");
        m_TooltipDesc.AddToClassList("tooltip-desc");
        AddToClassList("tooltip");
    }

    void SetValue(int price)
    {
        m_TooltipValue.text = "$" + price.ToString();
    }

    void SetWeight(float weight)
    {
        m_TooltipWeight.text = weight.ToString() + " kg";
    }

    void SetDescription(string description) 
    {
        m_TooltipDesc.text = description;
    }

    void SetTitle(string title)
    {
        m_TooltipTitle.text = title;
    }

    protected override void UpdateData()
    {
        SetValue(m_Item.Details.SellPrice);
        SetWeight(m_Item.Details.Weight);
        SetDescription(m_Item.Details.Description);
        SetTitle(m_Item.Details.FriendlyName);
    }
}

