using UnityEngine;

public class MothIdle : MonoBehaviour
{
    public Transform visual;

    public float swayAmount = 0.25f;
    public float swaySpeed = 2f;

    Vector3 startLocalPos;

    void OnEnable()
    {
        startLocalPos = visual.localPosition;
    }

    public void TickIdle()
    {
        Vector3 offset =
            Vector3.right *
            Mathf.Sin(Time.time * swaySpeed) *
            swayAmount;

        visual.localPosition =
            startLocalPos + offset;
    }
}