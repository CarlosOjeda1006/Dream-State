using UnityEngine;

public class DreamPuzzleManager : MonoBehaviour
{
    public Door[] doors;
    public GameObject[] drawings;

    public Door.DoorColor correctColor;
    public Door.DoorDir correctDir;

    void Start()
    {
        int random = Random.Range(0, drawings.Length);

        GameObject selected = drawings[random];
        selected.SetActive(true);

        string n = selected.name.ToLower();

        // COLOR
        if (n.Contains("blue")) correctColor = Door.DoorColor.Blue;
        if (n.Contains("red")) correctColor = Door.DoorColor.Red;
        if (n.Contains("green")) correctColor = Door.DoorColor.Green;
        if (n.Contains("yellow")) correctColor = Door.DoorColor.Yellow;
        if (n.Contains("purple")) correctColor = Door.DoorColor.Purple;
        if (n.Contains("orange")) correctColor = Door.DoorColor.Orange;

        // DIRECTION
        if (n.Contains("r")) correctDir = Door.DoorDir.Right;
        if (n.Contains("l")) correctDir = Door.DoorDir.Left;

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