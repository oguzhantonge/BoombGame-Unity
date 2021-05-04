using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public bool ismusicEnabled = true;

    public bool isfxEnabled = true;

    public AudioSource musicSource;
    public AudioClip moveSound;
    public AudioClip landSound;
    public AudioClip musicClips;
    public AudioClip gameOverClip;
    public AudioClip clearRowSound;

    [Range(0,1)]
    public float fxVolume = 1f;

    [Range(0, 1)]
    public float musicVolume = 1f;

    

    public IconToggle musicIconToggle;

    void Start()
    {
        PlayBackgroundMusic(musicClips);
    }
    

    // Update is called once per frame
    void Update()
    {
        // CheckMusicUpdate();
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!musicSource || !ismusicEnabled || !musicClip )
        {
            return;
        }

        musicSource.Stop();

        musicSource.clip = musicClip;

        musicSource.volume = 0.2f;

        musicSource.loop = true;

        musicSource.Play();

    }


    void CheckMusicUpdate()
    {
        if (musicSource.isPlaying != ismusicEnabled)
        {
            if (ismusicEnabled)
            {
                PlayBackgroundMusic(musicClips);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void ToggleMusic()
    {
        ismusicEnabled = !ismusicEnabled;
        CheckMusicUpdate();


        if (musicIconToggle)
        {

            musicIconToggle.ChangeIcon(ismusicEnabled);


        }

    }

}
