using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---- The Source ----")]
    [SerializeField] AudioSource SFX;

    [Header("---- The Effects ----")]
    public AudioClip playerHit; //done
    public AudioClip enemyHit; //done
    public AudioClip parrotThrow; //done
    public AudioClip anchorSwing; //done
    public AudioClip win; //done
    public AudioClip lose; //done
    public AudioClip enemyDie;


    public AudioClip treasureObtain; //done


    public void PlaySFX(AudioClip clip){
        SFX.PlayOneShot(clip);
    }


}
