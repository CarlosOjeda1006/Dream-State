using UnityEngine;
using UnityEngine.SceneManagement;

public class Out : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
        }
    }
}
