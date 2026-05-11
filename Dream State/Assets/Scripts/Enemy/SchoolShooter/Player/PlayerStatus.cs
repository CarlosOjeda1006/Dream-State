using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController controller;

    Coroutine slowRoutine;

    void Awake()
    {
        if (controller == null)
            controller = GetComponent<FirstPersonController>();
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (slowRoutine != null)
        {
            StopCoroutine(slowRoutine);
        }

        slowRoutine = StartCoroutine(
            SlowRoutine(multiplier, duration)
        );
    }

    IEnumerator SlowRoutine(float multiplier, float duration)
    {
        controller.speedMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        controller.speedMultiplier = 1f;

        slowRoutine = null;
    }
}