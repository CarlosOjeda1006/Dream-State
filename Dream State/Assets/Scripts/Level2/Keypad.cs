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

            keypadController.isSolved = true;

            if (animate)
            {
                ANI.SetBool("isOpen", true);
            }

            Exit();
        }
        else
        {
            SoundEffectManager.Play("WrongKeypad");
            textOB.text = "Wrong";
        }
    }

    public void Clear()
    {
        {
            textOB.text = "";
            SoundEffectManager.Play("PlayKeypad");
        }
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

}
