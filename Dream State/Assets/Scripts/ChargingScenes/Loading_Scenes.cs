using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_Scenes : MonoBehaviour
{
    /*public GameObject panelLoading;
    public Slider barraCarga;*/

    /*void Start()
    {
        panelLoading.SetActive(false);
    }*/

    public void NuevoJuego()
    {
        Load.SceneToLoad = "Level_01";
        SceneManager.LoadScene("LoadingScene");
    }

    /*IEnumerator CargarAsync(string nombreEscena)
    {
        panelLoading.SetActive(true);

        AsyncOperation operacion =
            SceneManager.LoadSceneAsync(nombreEscena);

        while (!operacion.isDone)
        {
            float progreso =
                Mathf.Clamp01(operacion.progress / 0.9f);

            barraCarga.value = progreso;

            yield return null;
        }
    }*/
}


