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

    public GameObject animateOB;
    public Animator ANI;

    public TMP_Text textOB;
    public string answer = "12345";

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
        if(textOB.text == answer)
        {
            SoundEffectManager.Play("CorrectKeypad");
            textOB.text = "Right";
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


    void Update()
    {
        if(textOB.text == "Right" && animate)
        {
            ANI.SetBool("isOpen", true);
            Debug.Log("Safe is open");
            keypadOB.SetActive(false);
        }
        
    }
}
