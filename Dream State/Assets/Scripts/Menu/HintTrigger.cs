using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [TextArea]
    public string hintMessage;
    public bool shouldDissapear;

    private void OnTriggerEnter(Collider other)
    {
        //Cambie el compareTag
        PlayerSingle player = other.GetComponent<PlayerSingle>();

        if (player != null)
        {
            InstructionsUI.Instance.ShowInstruction(hintMessage);
            SoundEffectManager.Play("Hint");

            if(shouldDissapear)
            gameObject.SetActive(false);
        }
    }
}