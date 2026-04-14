using UnityEngine;

public class Deactivate : MonoBehaviour
{
    public GameObject ojo1;
    public GameObject ojo2;
    public GameObject ojo3;

    private void Start()
    {
        ojo1.SetActive(false);
        ojo2.SetActive(false);
        ojo3.SetActive(false);
    }
}
