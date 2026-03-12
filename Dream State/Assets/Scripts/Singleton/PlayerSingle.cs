using UnityEngine;

public class PlayerSingle : MonoBehaviour
{
    public static PlayerSingle instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
