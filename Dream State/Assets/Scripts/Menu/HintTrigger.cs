using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [TextArea]
    public string hintMessage;

    private void OnTriggerEnter(Collider other)
    {
        //Cambie el compareTag
        PlayerSingle player = other.GetComponent<PlayerSingle>();

        if (player != null)
        {
            InstructionsUI.Instance.ShowInstruction(hintMessage);
            SoundEffectManager.Play("Hint");

            gameObject.SetActive(false);
        }
    }
}