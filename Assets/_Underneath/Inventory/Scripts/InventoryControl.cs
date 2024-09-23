using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryControl : OurMonoBehaviour
{
    private VisualElement m_Root; // Root visual element of the inventory, all elements in the UXML file are children of this.
    [SerializeField]
    private UIDocument m_Hud;
    private VisualElement m_Container;
    private Button m_InventoryButton;
    [SerializeField]
    private float m_OffscreenPos;
    private bool m_Hidden = true;

    // Start is called before the first frame update
    void Start()
    {        
        // Get the document and visual elements we will need
        UIDocument doc = GetComponentInChildren<UIDocument>();
        m_Root = doc.rootVisualElement;
        m_Container = m_Root.Q<VisualElement>("Container");
        m_InventoryButton = m_Hud.rootVisualElement.Q<Button>("Inventory");
        m_InventoryButton.RegisterCallback<ClickEvent>(ToggleInventory);

        Player.Instance.playerInput.onBagOpened += onToggleInventory;

        m_Container.style.left = Length.Percent(m_OffscreenPos);
    }

    private void OnDisable()
    {
        Player.Instance.playerInput.onBagOpened -= onToggleInventory;
    }

    private void ToggleInventory(ClickEvent evt = null)
    {
        GameManager.AudioManager.PlayOneShot(FMODEvents.Instance.OpenBag, Player.Instance.gameObject.transform.position);

        switch(m_Hidden)
        {
            case true:
                m_Container.style.left = 0f;
                m_Hidden = false;
                break;
            case false:
                m_Container.style.left = Length.Percent(m_OffscreenPos);
                m_Hidden = true;
                break;
        }
    }

    private void onToggleInventory()
    {
        ToggleInventory();
    }
}
