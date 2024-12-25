using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalletCounter : MonoBehaviour
{
    Label m_WalletCounter;

    // Start is called before the first frame update
    void Start()
    {
        m_WalletCounter = GetComponent<UIDocument>().rootVisualElement.Q<Label>("WalletCounter");

        RefreshWalletCounter();
        GameplayManager.Instance.m_RefreshMoney += RefreshWalletCounter;
    }

    public void RefreshWalletCounter()
    {
        m_WalletCounter.text = "$" + GameplayManager.Instance.GetMoney().ToString();
    }
}
