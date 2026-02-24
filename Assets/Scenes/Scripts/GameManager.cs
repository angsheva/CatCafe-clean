using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI coffeeText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI angryText;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Game Data")]
    public int coins = 0;
    public int coffee = 0;

    [Header("Lose Settings")]
    public int maxMissedCustomers = 5;
    private int missedCustomers = 0;

    [Header("Balance")]
    public int coffeeReward = 5;
    public float customerWaitTime = 10f;

    private bool isPaused = false;
    private bool isGameStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0f;

        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        UpdateUI();
        UpdateAngryUI();
    }

    // ▶ Запуск игры
    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        isGameStarted = true;
        isPaused = false;

        Time.timeScale = 1f;
    }

    // ⏸ Пауза
    public void TogglePause()
    {
        if (!isGameStarted) return;
        if (gameOverPanel.activeSelf) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }

    // ☕ Кофе
    public void AddCoffee(int amount)
    {
        if (isPaused || !isGameStarted) return;

        coffee += amount;
        ShowMessage("Готово");
        UpdateUI();
    }

    public bool HasCoffee()
    {
        return coffee > 0;
    }

    public void UseCoffee()
    {
        coffee--;
        UpdateUI();
    }

    // 💰 Монеты
    public void AddCoins(int amount)
    {
        if (isPaused || !isGameStarted) return;

        coins += amount;
        ShowMessage("+" + amount + " монет");
        UpdateUI();
    }

    // ❌ Клиент ушёл злым
    public void CustomerMissed()
    {
        if (!isGameStarted) return;

        missedCustomers++;
        UpdateAngryUI();

        if (missedCustomers >= maxMissedCustomers)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        isGameStarted = false;

        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    // 🔄 Рестарт
    public void RestartGame()
    {
        Time.timeScale = 1f;

        coins = 0;
        coffee = 0;
        missedCustomers = 0;

        isPaused = false;
        isGameStarted = true;

        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);

        UpdateUI();
        UpdateAngryUI();
    }

    void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = "Монеты: " + coins;

        if (coffeeText != null)
            coffeeText.text = "Кофе: " + coffee;
    }

    void UpdateAngryUI()
    {
        if (angryText != null)
        {
            angryText.text = "Злые клиенты: " + missedCustomers + " / " + maxMissedCustomers;

            // 🔥 подсветка перед проигрышем
            if (missedCustomers >= maxMissedCustomers - 1)
                angryText.color = Color.red;
            else
                angryText.color = Color.white;
        }
    }

    public void ShowMessage(string message)
    {
        if (messageText == null) return;

        messageText.text = message;
        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), 2f);
    }

    void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}