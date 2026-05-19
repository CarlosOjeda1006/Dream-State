using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [TextArea]
    public string hintMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InstructionsUI.Instance.ShowInstruction(hintMessage);
            SoundEffectManager.Play("Hint");

            gameObject.SetActive(false);
        }
    }
}