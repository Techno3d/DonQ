using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public float initialHeight = 0.5f;
    public TreasureSpawner parent;
    public int index;
    Transform player;
    ParticleSystem ps;

    AudioManager audioManager;

    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            initialHeight = hit.point.y;
        }
        transform.position = new Vector3(transform.position.x, initialHeight, transform.position.z);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ps = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().IncScore(1);
            audioManager.PlaySFX(audioManager.treasureObtain);
            parent.treasureList.Remove(gameObject);
            Destroy(gameObject); 
        }
    }
    
    void Update() {
        if(Vector3.Distance(transform.position, player.position) < 40 && !ps.isPlaying) {
            ps.Play();
        } else if(Vector3.Distance(transform.position, player.position) >= 50) {
            ps.Stop();
        }
    }
}
