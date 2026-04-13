using UnityEngine;
using System.Collections;

public class LinternaStun : MonoBehaviour
{

    public float range = 10f;
    public float stunDuration = 3f;
    public float cooldown = 1.5f;

    public KeyCode stunKey = KeyCode.F;

    [Header("Efecto de Flash")]
    public Light flashLight;
    Color originalColor;
    public float flashIntensity = 8f;
    public float flashDuration = 0.5f;

    public LinternaController linterna;

    private float nextUseTime;

    void Update()
    {
        if (Input.GetKeyDown(stunKey) && Time.time >= nextUseTime && linterna.isOn)
        {
            StartCoroutine(FlashAndStun());
            nextUseTime = Time.time + cooldown;
        }
    }

    IEnumerator FlashAndStun()
    {
        Transform cam = Camera.main.transform;
        RaycastHit hit;


        float originalIntensity = flashLight.intensity;
        float originalRange = flashLight.range;
        float originalAngle = flashLight.spotAngle;

        float targetIntensity = flashIntensity;
        float targetRange = originalRange * 2f;
        float targetAngle = originalAngle + 50f;

        float time = 0f;
        float durationUp = 0.03f;
        float durationDown = 0.4f;

        originalColor = flashLight.color;
        flashLight.color = Color.cyan;

        while (time < durationUp)
        {
            time += Time.deltaTime;

            float t = time / durationUp;

            flashLight.intensity = Mathf.Lerp(originalIntensity, targetIntensity, t);
            flashLight.range = Mathf.Lerp(originalRange, targetRange, t);
            flashLight.spotAngle = Mathf.Lerp(originalAngle, targetAngle, t);

            yield return null;
        }

        if (Physics.Raycast(cam.position, cam.forward, out hit, range))
        {
            if (hit.collider.TryGetComponent<IStunneable>(out var stunneable))
            {
                stunneable.Stun(stunDuration);
            }
        }

        time = 0f;

        while (time < durationDown)
        {
            time += Time.deltaTime;

            float t = time / durationDown;


            flashLight.intensity = Mathf.Lerp(targetIntensity, originalIntensity, t);
            flashLight.range = Mathf.Lerp(targetRange, originalRange, t);
            flashLight.spotAngle = Mathf.Lerp(targetAngle, originalAngle, t);

            yield return null;
        }

        flashLight.color = originalColor;
        flashLight.intensity = originalIntensity;
        flashLight.range = originalRange;
        flashLight.spotAngle = originalAngle;
    }
}