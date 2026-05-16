using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Keypad : MonoBehaviour
{
    public OpenKeypad keypadController;
    public GameObject player;
    public GameObject keypadOB;
    public GameObject hud;

    public SymbolPuzzleLogic puzzleLogic;
    public GameObject notesTab;
    public bool canOpenDoors = false;

    public GameObject animateOB;
    public Animator ANI;
    
    public TMP_Text textOB;

    public bool animate;

    [Header("Text Message")]
    public static string instructionsText;
    public static bool uiActive;
    [SerializeField] GameObject instructionsBox;

    private void Start()
    {
        keypadOB.SetActive(false);
        
    }
    public void Number(int number)
    {
        textOB.text += number.ToString(); //para que los números salgan consecutivamente
        SoundEffectManager.Play("PlayKeypad");
    }
    public void Execute()
    {
        if (textOB.text == puzzleLogic.correctAnswer)
        {
            SoundEffectManager.Play("CorrectKeypad");
            textOB.text = "Right";

            notesTab.SetActive(true);
            puzzleLogic.correctSol.SetActive(true);
            Debug.Log(puzzleLogic.correctSol);
            canOpenDoors = true;
            StartCoroutine(SolveSequence());
        }
        else
        {
            SoundEffectManager.Play("WrongKeypad");
            textOB.text = "Wrong";

            Invoke(nameof(Clear), 1f);
        }
    }

    public void Clear()
    {
        textOB.text = "";
        SoundEffectManager.Play("PlayKeypad");
    }

    public void Exit()
    {
        keypadOB.SetActive(false);
        hud.SetActive(true);

        player.GetComponent<FirstPersonController>().enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        keypadController.isOpen = false;
    }
    IEnumerator SolveSequence()
    {
        keypadController.isSolved = true;

        if (animate && ANI != null)
        {
            ANI.SetBool("isOpen", true);
        }

        instructionsBox.SetActive(true);

        instructionsBox.GetComponent<TMP_Text>().text = "New Clue Added [Press TAB to view].";

        SoundEffectManager.Play("Clue");

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);

        Exit();
    }

}
