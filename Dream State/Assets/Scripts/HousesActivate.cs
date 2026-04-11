using UnityEngine;

public class HousesActivate : MonoBehaviour
{
    public GameObject objectToActivateL;
    public GameObject objectToActivateR;

    private void Start()
    {
        objectToActivateL.SetActive(true);
        objectToActivateR.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectToActivateL.SetActive(false);
            objectToActivateR.SetActive(true);
        }
    }
}
