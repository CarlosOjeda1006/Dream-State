using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] ItemLocations;

    void Start()
    {
        if (ItemLocations.Length == 0) return;

        foreach (GameObject obj in ItemLocations)
        {
            obj.SetActive(false);
        }

        int random = Random.Range(0, ItemLocations.Length);
        ItemLocations[random].SetActive(true);
    }
}