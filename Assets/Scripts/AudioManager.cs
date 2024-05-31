using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    public void CorrectAnswerAudio()
    {
            Debug.Log("3");
            audioSource.clip = correctAudio;
            audioSource.Play();  
    }


    public void WrongAnswerAudio()
    {
            Debug.Log("4");
            audioSource.clip = wrongAudio;
            audioSource.Play(); 
    }



}
