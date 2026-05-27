using System.Collections;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light luz;
    public float minTime = 0.05f;
    public float maxTime = 0.2f;

    public void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            luz.enabled = !luz.enabled;
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}