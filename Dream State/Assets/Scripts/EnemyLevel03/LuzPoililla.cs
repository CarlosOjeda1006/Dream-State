using System.Collections.Generic;
using UnityEngine;

public class LuzPoililla : MonoBehaviour
{
    public static List<LuzPoililla> AllLights = new List<LuzPoililla>();

    public Light lightSource;

    void OnEnable()
    {
        if (!AllLights.Contains(this))
            AllLights.Add(this);
    }

    void OnDisable()
    {
        AllLights.Remove(this);
    }
}
