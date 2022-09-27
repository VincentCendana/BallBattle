using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Manager Component")]
    public static AudioManager instance;    // instance of the audio manager
    public AudioSource source;              // game scene's audio source

    [Header("Audio Clips")]
    public AudioClip soldierSpawn;          // audio for spawning soldiers
    public AudioClip carryBall;             // audio for when an attacker caught a ball
    public AudioClip catchAttacker;         // audio for when a defender successfully caught an attacker carrying the ball
    public AudioClip passBall;              // audio for when the caught attacker passes the ball to other attacker
    public AudioClip goal;                  // audio for when the ball reaches the target gate
    public AudioClip reachFence;            // audio for when a soldier reaches their opponent's fence
    public AudioClip timesUp;               // audio for when the timer runs out
    public AudioClip clickButton;           // audio for UI clicks
    public AudioClip sliderClick;           // audio for slider button clicks

    private void Awake()
    {
        // ensure only one instance of audio manager is instantiated
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // play audio for UI button clicks
    public void PlayUIClick()
    {
        PlayAudio(clickButton, 0.25f);
    }

    // play audio for UI slider clicks
    public void PlaySliderClick()
    {
        PlayAudio(sliderClick, 0.25f);
    }

    // play audio at default volume
    public void PlayDefaultAudio(AudioClip clip)
    {
        PlayAudio(clip, 0.25f);
    }

    // set clip to audio source to play
    public void PlayAudio(AudioClip clip, float volume)
    {
        // set source's clip and volume
        source.volume = volume;
        source.clip = clip;

        // play audio through the source
        source.Play();
    }
}
