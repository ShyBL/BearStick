using UnityEngine;
using UnityEngine.SceneManagement;
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
        PlayerData.Instance.IncreaseExpenses(BaseExpenses);
        m_Document.rootVisualElement.style.display = DisplayStyle.None;

        
    }

    void ExitPressed(ClickEvent evt)
    {
        m_Document.rootVisualElement.style.display = DisplayStyle.None;
        //when this is clicked, need to load a new day
        //StartOfDay.Instance.StartNewDay();

        //This is how you would reset the scene, but you would need some form of button or something 
        //So that the scene does not instantly reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndDay()
    {
        m_Document.rootVisualElement.style.display = DisplayStyle.Flex;

        m_Title.text = "Day " + PlayerData.Instance.GetDayCount();
        m_PrevMoneyLabel.text = "$" + PlayerData.Instance.GetMoney().ToString();
        m_EarnedMoneyLabel.text = "$" + PlayerData.Instance.GetMoneyEarned().ToString();

        PlayerData.Instance.DecreaseMoney(PlayerData.Instance.GetExpenses());
        PlayerData.Instance.ApplyMoneyChange();

        m_ExpensesLabel.text = "$" + PlayerData.Instance.GetExpenses().ToString();
        m_TotalMoneyLabel.text = "$" + PlayerData.Instance.GetMoney().ToString();

        PlayerData.Instance.IncrementDayCount();

        if (PlayerData.Instance.GetDayCount() % IncreaseFrequency == 0)
            PlayerData.Instance.IncreaseExpenses(ExpensesIncrease);

        //Set the player's spawn location for the next day
        Player.Instance.SetPlayerSpawn(PlayerData.Instance.v_SpawnLocation);
        //Save Game
        SavingAndLoading.Instance.SavePlayerInformation();
;
    }

}
