using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndOfDay : MonoBehaviour
{
    public int BaseExpenses;
    public int ExpensesIncrease;
    public int IncreaseFrequency;

    private UIDocument m_Document;
    private Label m_PrevMoneyLabel;
    private Label m_TotalMoneyLabel;
    private Label m_EarnedMoneyLabel;
    private Label m_ExpensesLabel;

    void Start()
    {
        m_Document = GetComponent<UIDocument>();
        m_PrevMoneyLabel = m_Document.rootVisualElement.Q<Label>("PrevMoney");
        m_TotalMoneyLabel = m_Document.rootVisualElement.Q<Label>("TotalMoney");
        m_EarnedMoneyLabel = m_Document.rootVisualElement.Q<Label>("EarnedMoney");
        m_ExpensesLabel = m_Document.rootVisualElement.Q<Label>("Expenses");
        PlayerData.Instance.IncreaseExpenses(BaseExpenses);
        m_Document.rootVisualElement.style.display = DisplayStyle.None;
        EndDay();
    }

    public void EndDay()
    {
        m_Document.rootVisualElement.style.display = DisplayStyle.Flex;

        m_PrevMoneyLabel.text = "$" + PlayerData.Instance.GetMoney().ToString();
        m_EarnedMoneyLabel.text = "$" + PlayerData.Instance.GetMoneyEarned().ToString();

        if (PlayerData.Instance.GetDayCount() % IncreaseFrequency == 0)
            PlayerData.Instance.IncreaseExpenses(ExpensesIncrease);

        PlayerData.Instance.DecreaseMoney(PlayerData.Instance.GetExpenses());
        PlayerData.Instance.ApplyMoneyChange();

        m_ExpensesLabel.text = "$" + PlayerData.Instance.GetExpenses().ToString();
        m_TotalMoneyLabel.text = "$" + PlayerData.Instance.GetMoney().ToString();

    }
}
