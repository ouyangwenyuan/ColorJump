using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public AudioSource audioSource;
    public enum Type { MainMusic };
    public static Music instance;

    [HideInInspector]
    public AudioClip[] musicClips;

    private Type currentType = Type.MainMusic;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public bool IsMuted()
    {
        return !IsEnabled();
    }

    public bool IsEnabled()
    {
        return CPlayerPrefs.GetBool("music_enabled", true);
    }

    public void SetEnabled(bool enabled, bool updateMusic = false)
    {
        CPlayerPrefs.SetBool("music_enabled", enabled);
        if (updateMusic)
            UpdateSetting();
    }

    public void Play(Music.Type type)
    {
        if (currentType != type || !audioSource.isPlaying)
        {
            StartCoroutine(PlayNewMusic(type));
        }
    }

    private IEnumerator PlayNewMusic(Music.Type type)
    {
        while (audioSource.volume >= 0.1f)
        {
            audioSource.volume -= 0.2f;
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        audioSource.clip = musicClips[(int)type];
        if (IsEnabled())
        {
            currentType = type;
            audioSource.Play();
        }
        audioSource.volume = 1;
    }

    private void UpdateSetting()
    {
        if (audioSource == null) return;
        if (IsEnabled())
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();
    }
}
