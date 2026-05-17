using UnityEngine;

public class PortraitPuzzle : MonoBehaviour
{
    public DoorL3[] doors;
    public GameObject[] portraits;

    [HideInInspector]
    public string correctAnswer;

    public DoorL3.DoorNumber correctNumber;

    void Start()
    {
        if (portraits.Length == 0)
        {
            Debug.LogError("No hay portraits asignados");
            return;
        }

        int random = Random.Range(0, portraits.Length);
        GameObject selected = portraits[random];

        selected.SetActive(true);

        DataHandlingL3 data = selected.GetComponent<DataHandlingL3>();

        if (data == null)
        {
            Debug.LogError("El portrait no tiene PortraitData: " + selected.name);
            return;
        }

        correctNumber = data.number;

        Debug.Log("Selected portrait: " + selected.name);
        Debug.Log("Correct Door Number: " + correctNumber);

        SetCorrectDoor();
    }

    void SetCorrectDoor()
    {
        foreach (DoorL3 door in doors)
        {
            bool correct =
                door.doorNumber == correctNumber;

            door.SetCorrect(correct);

        }
    }
}
