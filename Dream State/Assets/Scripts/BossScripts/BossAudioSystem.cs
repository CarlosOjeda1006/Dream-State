using UnityEngine;

public class BossAudioSystem : MonoBehaviour
{
    public void PlayDirectionSound(
        BossDirectionPoint.Direction dir)
    {
        switch (dir)
        {
            case BossDirectionPoint.Direction.North:
                //Audio norte
                break;

            case BossDirectionPoint.Direction.South:
                //Audio sur
                break;

            case BossDirectionPoint.Direction.East:
                //Audio este
                break;

            case BossDirectionPoint.Direction.West:
                //Audio oeste
                break;
        }
    }
}