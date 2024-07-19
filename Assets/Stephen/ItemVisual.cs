using UnityEngine.UIElements;

public class ItemVisual : VisualElement
{
    private readonly Item m_Item;

    public ItemVisual(Item item)
    {
        m_Item = item;

        // Name it based on the user friendly name of the item.
        name = $"{m_Item.FriendlyName}";
        // Set it to hidden at first so it doesn't show up till we find a spot for it in inventory.
        style.visibility = Visibility.Hidden;

        // Create the actual icon image child element
        VisualElement icon = new VisualElement
        {
            style = {
                backgroundImage = m_Item.Icon.texture,
                // TODO in the future if we support bigger items change this as this only works with 1x1 items
                // Set the width and length depending on the size of the object using percent as pixel had floating point inaccuracies.
                width = Length.Percent(100 * item.SlotDimension.Width),
                height = Length.Percent(100 * item.SlotDimension.Height)
            }
        };
        Add(icon);

        // Add the selectors to the elements created so the correct styles are applied
        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");
    }
}
