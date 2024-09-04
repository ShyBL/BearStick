using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalletCounter : MonoBehaviour
{
    public static WalletCounter Instance;

    Label m_WalletCounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_WalletCounter = GetComponent<UIDocument>().rootVisualElement.Q<Label>("WalletCounter");

        RefreshWalletCounter(PlayerData.Instance.GetMoney());
    }

    public void RefreshWalletCounter(int money)
    {
        m_WalletCounter.text = "$" + money.ToString();
    }
}
