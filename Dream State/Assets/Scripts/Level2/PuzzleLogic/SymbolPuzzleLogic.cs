using UnityEngine;

public class SymbolPuzzleLogic : MonoBehaviour
{
    public DoorsL2[] doors;
    public GameObject[] combinations;
    public GameObject[] solutions;

    [HideInInspector]
    public string correctAnswer;
    [HideInInspector]
    public GameObject correctSol;

    public DoorsL2.DoorSymbol correctSymbol;

    void Start()
    {
        if (combinations.Length == 0)
        {
            Debug.LogError("No hay boards asignados");
            return;
        }

        int random = Random.Range(0, combinations.Length);
        GameObject selected = combinations[random];

        selected.SetActive(true);

        DataHandlingL2 data = selected.GetComponent<DataHandlingL2>();

        if (data == null)
        {
            Debug.LogError("El combination no tiene ComboData: " + selected.name);
            return;
        }

        correctSymbol = data.symbol;

        Debug.Log("Selected combo: " + selected.name);
        Debug.Log("Correct symbol: " + correctSymbol);

        SetCorrectDoor();
    }

    void SetCorrectDoor()
    {
        foreach (DoorsL2 door in doors)
        {
            bool correct =
                door.doorSymbol == correctSymbol;

            door.SetCorrect(correct);

            Debug.Log(door.name + " is correct: " + correct);
        }

        switch (correctSymbol)
        {
            case DoorsL2.DoorSymbol.Moth:
                correctAnswer = "3156";
                correctSol = solutions[0];
                break;
            case DoorsL2.DoorSymbol.Sun:
                correctAnswer = "4718";
                correctSol = solutions[1];
                break;
            case DoorsL2.DoorSymbol.Cat:
                correctAnswer = "0539";
                correctSol = solutions[2];
                break;
            case DoorsL2.DoorSymbol.Moon:
                correctAnswer = "5463";
                correctSol = solutions[3];
                break;
            case DoorsL2.DoorSymbol.Spider:
                correctAnswer = "2587";
                correctSol = solutions[4];
                break;
            case DoorsL2.DoorSymbol.Eye:
                correctAnswer = "7356";
                correctSol = solutions[5];
                break;
            case DoorsL2.DoorSymbol.Hand:
                correctAnswer = "9541";
                correctSol = solutions[6];
                break;
            case DoorsL2.DoorSymbol.Snake:
                correctAnswer = "3260";
                correctSol = solutions[7];
                break;
            case DoorsL2.DoorSymbol.Dragon:
                correctAnswer = "7912";
                correctSol = solutions[8];
                break;
        }
        Debug.Log(correctAnswer);
    }
}
