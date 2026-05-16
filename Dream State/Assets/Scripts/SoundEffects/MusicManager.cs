using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(OnValueChanged);

            SetVolume(musicSlider.value);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChanged(float value)
    {
        SetVolume(value);
    }
}