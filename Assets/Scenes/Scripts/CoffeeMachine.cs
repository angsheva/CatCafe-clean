using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.AddCoffee(1);
    }
}
