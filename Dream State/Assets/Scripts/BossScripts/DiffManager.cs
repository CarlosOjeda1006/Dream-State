using UnityEngine;

public class DiffManager : MonoBehaviour
{
    public static DiffManager Instance;

    [Header("Counters")]
    public int flashlightUsed;
    public int bottlesThrown;
    public int deaths;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}