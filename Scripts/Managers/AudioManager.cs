using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] bool MultipleMusicClips;
    [SerializeField] bool MultipleClickSounds;
    [SerializeField] bool MultipleHoverSounds;

    [Header("Music files")]
    [SerializeField] AudioClip[] Music;
    [SerializeField] AudioClip[] Click;
    [SerializeField] AudioClip[] Hover;

    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource MouseClickSource;
    [SerializeField] AudioSource MouseHoverSource;


    [Header("Progress Manager")]
    [SerializeField] ProgressManager PManager;
    [SerializeField] int CurrentChapter;
    void Start()
    {
        PManager = FindObjectOfType<ProgressManager>();
        MouseHoverSource.clip = Hover[0];
        MouseClickSource.clip = Click[0];
        if (PManager.ProgressiveMenu)
        {
            CurrentChapter = PManager.CheckProgress();
        }
        else
        {
            CurrentChapter = 0; //if not progressive then play first clip; 
        }
        PlayM(CurrentChapter);
    }
    void PlayM(int MusicIndex)
    {
        MusicSource.clip = Music[MusicIndex];
        MusicSource.Play();
    }
    public void PlayClickSound()
    {
        if (MultipleClickSounds)
        {
            MouseClickSource.clip = Click[Random.Range(0, Click.Length)];
            MouseClickSource.Play();
        }
        else
        {
            MouseClickSource.Play();
        }
    }
    public AudioClip HoverSound()
    {
        AudioClip clip;
            if (MultipleHoverSounds)
            {
                clip = Hover[Random.Range(0, Hover.Length)];
            }
            else
            {
                clip = Hover[0];
            }

        return clip;
    }
}
