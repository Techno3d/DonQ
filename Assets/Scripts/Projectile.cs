using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1f;
    public string funnyTag = "Player";
    public bool isAnchored = false;
    float WaitTime = 0f;
    float rotation = 360f;
    AudioManager audioManager;
    
     private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(funnyTag))
        {
            Health health = collision.collider.GetComponent<Health>();
            if (health != null)
            {
                if (funnyTag == "Enemy"){
                    audioManager.PlaySFX(audioManager.enemyHit);
                }
                if (funnyTag == "Player"){
                    audioManager.PlaySFX(audioManager.playerHit);
                }
                health.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(funnyTag))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
    
    void Update() {
        if(isAnchored) {
            transform.Rotate(new Vector3(0, rotation*Time.deltaTime, 0));
            WaitTime += Time.deltaTime;
            if(WaitTime > 360f/rotation) {
                Destroy(gameObject);
            }
        }
    }
}