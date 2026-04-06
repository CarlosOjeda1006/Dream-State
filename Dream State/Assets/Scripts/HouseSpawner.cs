using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    public GameObject[] houses;
    public Transform spawnL;
    public Transform spawnR;

    void Start()
    {
        SpawnHouse(spawnL);
        SpawnHouse(spawnR);
    }

    void SpawnHouse(Transform spawnPoint)
    {
        int randomIndex = Random.Range(0, houses.Length);
        Instantiate(houses[randomIndex], spawnPoint.position, spawnPoint.rotation, transform);
    }
}