using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableDistractionObject : MonoBehaviour
{
    public float distractionRadius = 12f;
    public float distractionDuration = 4f;
    public float minImpactVelocity = 2f;
    public float impactCooldown = 0.5f;

    Rigidbody rb;
    bool isHeld;
    float nextImpactTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetHeld(bool held)
    {
        isHeld = held;

        rb.isKinematic = held;
        rb.useGravity = !held;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.detectCollisions = !held;
    }

    public void Drop()
    {
        SetHeld(false);
        transform.SetParent(null);
    }

    public void Throw(Vector3 direction, float force)
    {
        SetHeld(false);
        transform.SetParent(null);

        rb.detectCollisions = true;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHeld) return;
        if (Time.time < nextImpactTime) return;
        if (collision.relativeVelocity.magnitude < minImpactVelocity) return;

        nextImpactTime = Time.time + impactCooldown;

        NotifyDistraction();
    }

    void NotifyDistraction()
    {
        SpiderDistractionController[] spiders =
            Object.FindObjectsByType<SpiderDistractionController>(FindObjectsSortMode.None);

        Vector3 pos = transform.position;
        float radiusSqr = distractionRadius * distractionRadius;

        for (int i = 0; i < spiders.Length; i++)
        {
            Vector3 diff = spiders[i].transform.position - pos;

            if (diff.sqrMagnitude <= radiusSqr)
            {
                spiders[i].HearDistraction(pos, distractionDuration);
            }
        }
    }
    void OnPickup(Transform holdPoint);
    void OnDrop(Vector3 force);
}