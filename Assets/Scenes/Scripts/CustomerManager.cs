using UnityEngine;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    public GameObject customerPrefab;
    public Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnCustomer(i);
        }
    }

    public void SpawnCustomer(int spawnIndex)
    {
        GameObject customer = Instantiate(
            customerPrefab,
            spawnPoints[spawnIndex].position,
            Quaternion.identity
        );

        customer.GetComponent<Customer>().spawnIndex = spawnIndex;
    }

    public void RespawnCustomer(int spawnIndex)
    {
        StartCoroutine(SpawnAfterDelay(spawnIndex));
    }

    IEnumerator SpawnAfterDelay(int index)
    {
        yield return new WaitForSeconds(1.5f);
        SpawnCustomer(index);
    }



    //private int pendingSpawnIndex;

   // void SpawnDelayed()
   // {
   //     SpawnCustomer(pendingSpawnIndex);
    //}
}
