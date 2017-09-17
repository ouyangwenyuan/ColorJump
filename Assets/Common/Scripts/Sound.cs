using UnityEngine;
using System.Collections;
using System;

public class Sound : MonoBehaviour
{
    public AudioSource audioSource;
    public enum Button { Default };
    public enum Others { Score, Fail, Jump};

    [HideInInspector]
    public AudioClip[] buttonClips;
    [HideInInspector]
    public AudioClip[] otherClips;

    public static Sound instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateSetting();
    }

    public bool IsMuted()
    {
        return !IsEnabled();
    }

    public bool IsEnabled()
    {
        return CPlayerPrefs.GetBool("sound_enabled", true);
    }

    public void SetEnabled(bool enabled)
    {
        CPlayerPrefs.SetBool("sound_enabled", enabled);
        UpdateSetting();
    }

    public void Play(AudioClip clip)
    {
        if (IsEnabled())
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void Play(AudioSource audioSource)
    {
        if (IsEnabled())
        {
            audioSource.Play();
        }
    }

    public void PlayButton(Button type = Button.Default)
    {
        int index = (int)type;
        Play(buttonClips[index]);
    }

    public void Play(Others type)
    {
        int index = (int)type;
        Play(otherClips[index]);
    }

    public void UpdateSetting()
    {
        audioSource.mute = IsMuted();
    }
}