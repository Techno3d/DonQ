using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---- The Source ----")]
    [SerializeField] AudioSource SFX;

    [Header("---- The Effects ----")]
    public AudioClip playerHit;
    public AudioClip enemyHit;
    public AudioClip parrotThrow;
    public AudioClip anchorSwing;
    public AudioClip win;
    public AudioClip lose;


    public void PlaySFX(AudioClip clip){
        SFX.PlayOneShot(clip);
    }


}
