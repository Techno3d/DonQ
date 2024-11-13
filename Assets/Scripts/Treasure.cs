using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public float initialHeight = 0.5f;
    public TreasureSpawner parent;
    public int index;

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
}
