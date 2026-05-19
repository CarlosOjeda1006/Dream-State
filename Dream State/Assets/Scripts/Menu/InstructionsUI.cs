using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    public static InstructionsUI Instance;

    public GameObject instructionsBox;
    public TMP_Text instructionsText;

    Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
    }

    public void ShowInstruction(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(InstructionRoutine(message));
    }

    IEnumerator InstructionRoutine(string message)
    {
        instructionsBox.SetActive(true);

        instructionsText.text = message;

        yield return new WaitForSeconds(2f);

        instructionsBox.SetActive(false);

        currentRoutine = null;
    }
}