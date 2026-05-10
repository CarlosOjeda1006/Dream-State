using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableDistractionObject : MonoBehaviour
{
    public float distractionRadius = 12f;
    public float distractionDuration = 4f;
    public float minImpactVelocity = 2f;
    public float impactCooldown = 0.5f;

    Rigidbody rb;
    Collider[] objectColliders;
    bool isHeld;
    float nextImpactTime;

    public Rigidbody Rigidbody => rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        objectColliders = GetComponentsInChildren<Collider>();
    }

    public void SetHeld(bool held)
    {
        isHeld = held;

        rb.isKinematic = held;
        rb.useGravity = !held;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        for (int i = 0; i < objectColliders.Length; i++)
            objectColliders[i].enabled = !held;
    }

    public void Throw(Vector3 direction, float force)
    {
        SetHeld(false);
        transform.parent = null;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public void Drop()
    {
        SetHeld(false);
        transform.parent = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHeld)
            return;

        if (Time.time < nextImpactTime)
            return;

        if (collision.relativeVelocity.magnitude < minImpactVelocity)
            return;

        nextImpactTime = Time.time + impactCooldown;
        NotifyDistraction();
    }

    void NotifyDistraction()
    {
        SpiderDistractionController[] spiders = Object.FindObjectsByType<SpiderDistractionController>(FindObjectsSortMode.None);

        for (int i = 0; i < spiders.Length; i++)
        {
            float distanceSqr = (spiders[i].transform.position - transform.position).sqrMagnitude;

            if (distanceSqr <= distractionRadius * distractionRadius)
                spiders[i].HearDistraction(transform.position, distractionDuration);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distractionRadius);
    }
}