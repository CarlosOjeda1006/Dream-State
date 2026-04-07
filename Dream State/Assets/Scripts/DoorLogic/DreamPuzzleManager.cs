using UnityEngine;

public class DreamPuzzleManager : MonoBehaviour
{
    public Door[] doors;
    public GameObject[] drawings;

    private Door.DoorColor correctColor;
    private Door.DoorDir correctDir;

    void Start()
    {
        if (drawings.Length == 0)
        {
            Debug.LogError("No hay drawings asignados");
            return;
        }

        int random = Random.Range(0, drawings.Length);
        GameObject selected = drawings[random];

        selected.SetActive(true);

        DataHandling data = selected.GetComponent<DataHandling>();

        if (data == null)
        {
            Debug.LogError("El drawing no tiene DrawingData: " + selected.name);
            return;
        }

        correctColor = data.color;
        correctDir = data.dir;

        Debug.Log("Selected drawing: " + selected.name);
        Debug.Log("Correct color: " + correctColor);
        Debug.Log("Correct direction: " + correctDir);

        SetCorrectDoor();
    }

    void SetCorrectDoor()
    {
        foreach (Door door in doors)
        {
            bool correct =
                door.doorColor == correctColor &&
                door.doorDir == correctDir;

            door.SetCorrect(correct);

            Debug.Log(door.name + " is correct: " + correct);
        }
    }
}