using UnityEngine;

public class SFXVolumeFollower : MonoBehaviour
{
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        source.volume = SoundEffectManager.sfxVolume;
    }
}