using UnityEngine;
using UnityEngine.UIElements;

public class EndOfDay : MonoBehaviour
{
    public static EndOfDay Instance;
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
    
    public int BaseExpenses;
    public int ExpensesIncrease;
    public int IncreaseFrequency;

    private UIDocument m_Document;
    private Label m_PrevMoneyLabel;
    private Label m_TotalMoneyLabel;
    private Label m_EarnedMoneyLabel;
    private Label m_ExpensesLabel;
    private Label m_Title;

    void Start()
    {
        m_Document = GetComponent<UIDocument>();
        m_PrevMoneyLabel = m_Document.rootVisualElement.Q<Label>("PrevMoney");
        m_TotalMoneyLabel = m_Document.rootVisualElement.Q<Label>("TotalMoney");
        m_EarnedMoneyLabel = m_Document.rootVisualElement.Q<Label>("EarnedMoney");
        m_ExpensesLabel = m_Document.rootVisualElement.Q<Label>("Expenses");
        m_Title = m_Document.rootVisualElement.Q<Label>("Title");
        m_Document.rootVisualElement.Q<Button>("Exit").RegisterCallback<ClickEvent>(ExitPressed);
        GameplayManager.Instance.IncreaseExpenses(BaseExpenses);
        m_Document.rootVisualElement.style.display = DisplayStyle.None;
    }

    void ExitPressed(ClickEvent evt)
    {
        m_Document.rootVisualElement.style.display = DisplayStyle.None;
        StartOfDay.Instance.StartNewDay();
    }

    public void EndDay()
    {
        m_Document.rootVisualElement.style.display = DisplayStyle.Flex;

        m_Title.text = "Day " + GameplayManager.Instance.GetDayCount();
        m_PrevMoneyLabel.text = "$" + GameplayManager.Instance.GetMoney().ToString();
        m_EarnedMoneyLabel.text = "$" + GameplayManager.Instance.GetMoneyEarned().ToString();

        if (GameplayManager.Instance.GetDayCount() % IncreaseFrequency == 0)
            GameplayManager.Instance.IncreaseExpenses(ExpensesIncrease);

        GameplayManager.Instance.DecreaseMoney(GameplayManager.Instance.GetExpenses());
        GameplayManager.Instance.ApplyMoneyChange();

        m_ExpensesLabel.text = "$" + GameplayManager.Instance.GetExpenses().ToString();
        m_TotalMoneyLabel.text = "$" + GameplayManager.Instance.GetMoney().ToString();

        GameplayManager.Instance.IncrementDayCount();
        //SavingAndLoading.Instance.SavePlayerInformation();
    }
}
