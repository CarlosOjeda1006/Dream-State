using UnityEngine;

public interface IPickable
{
    void OnPickup(Transform holdPoint);
    void OnDrop(Vector3 force);
}