using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryControl : OurMonoBehaviour
{
    private VisualElement m_Root; // Root visual element of the inventory, all elements in the UXML file are children of this.
    [SerializeField]
    private UIDocument m_Hud;
    private Button m_InventoryButton;

    // Start is called before the first frame update
    void Start()
    {        
        // Get the document and visual elements we will need
        UIDocument doc = GetComponentInChildren<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_InventoryButton = m_Hud.rootVisualElement.Q<Button>("Inventory");
        m_InventoryButton.RegisterCallback<ClickEvent>(ToggleInventory);

        Player.Instance.playerInput.onBagOpened += onToggleInventory;

        m_Root.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        Player.Instance.playerInput.onBagOpened -= onToggleInventory;
    }

    private void ToggleInventory(ClickEvent evt = null)
    {
        GameManager.AudioManager.PlayOneShot(FMODEvents.Instance.OpenBag, Player.Instance.gameObject.transform.position);

        switch(m_Root.resolvedStyle.display)
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
}
