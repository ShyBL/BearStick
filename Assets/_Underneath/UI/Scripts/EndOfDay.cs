using UnityEngine;
using UnityEngine.UIElements;

public class EndOfDay : OurMonoBehaviour
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
        GameManager.GameplayManager.IncreaseExpenses(BaseExpenses);
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

        m_Title.text = "Day " + GameManager.GameplayManager.GetDayCount();
        m_PrevMoneyLabel.text = "$" + GameManager.GameplayManager.GetMoney().ToString();
        m_EarnedMoneyLabel.text = "$" + GameManager.GameplayManager.GetMoneyEarned().ToString();

        if (GameManager.GameplayManager.GetDayCount() % IncreaseFrequency == 0)
            GameManager.GameplayManager.IncreaseExpenses(ExpensesIncrease);

        GameManager.GameplayManager.DecreaseMoney(GameManager.GameplayManager.GetExpenses());
        GameManager.GameplayManager.ApplyMoneyChange();

        m_ExpensesLabel.text = "$" + GameManager.GameplayManager.GetExpenses().ToString();
        m_TotalMoneyLabel.text = "$" + GameManager.GameplayManager.GetMoney().ToString();

        GameManager.GameplayManager.IncrementDayCount();
        //SavingAndLoading.Instance.SavePlayerInformation();
    }
}
