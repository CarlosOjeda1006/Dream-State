using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;
    //so we can get it from any script but only set it from this one

    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;
    }
}
