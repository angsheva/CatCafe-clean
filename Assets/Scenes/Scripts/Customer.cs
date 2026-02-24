using UnityEngine;

public class Customer : MonoBehaviour
{
    public GameObject orderBubble;
    public Transform timerFill;

    [HideInInspector]
    public int spawnIndex;

    private float maxWaitTime;
    private float currentWaitTime;

    private bool isWaiting = true;
    private bool isHandled = false;

    private void Start()
    {
        maxWaitTime = GameManager.Instance.customerWaitTime;
        currentWaitTime = maxWaitTime;

        orderBubble.SetActive(true);
    }

    private void Update()
    {
        if (!isWaiting) return;

        currentWaitTime -= Time.deltaTime;

        if (timerFill != null)
        {
            float percent = currentWaitTime / maxWaitTime;
            timerFill.localScale = new Vector3(percent, 1f, 1f);
        }

        if (currentWaitTime <= 0f)
        {
            LeaveAngry();
        }
    }

    private void OnMouseDown()
    {
        if (isHandled) return;

        if (!GameManager.Instance.HasCoffee())
        {
            GameManager.Instance.ShowMessage("Нет кофе!");
            return;
        }

        isHandled = true;

        GameManager.Instance.UseCoffee();
        CompleteOrder();
    }

    void CompleteOrder()
    {
        isWaiting = false;
        orderBubble.SetActive(false);

        GameManager.Instance.AddCoins(GameManager.Instance.coffeeReward);

        int index = spawnIndex;
        Destroy(gameObject);

        CustomerManager.Instance.RespawnCustomer(index);
    }

    void LeaveAngry()
    {
        isWaiting = false;
        orderBubble.SetActive(false);
        GameManager.Instance.CustomerMissed(); // 🔥 ВАЖНО
        int index = spawnIndex;
        Destroy(gameObject);
        CustomerManager.Instance.RespawnCustomer(index);
    }
}
