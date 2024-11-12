using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField]
    Transform player;
    GameManager manager;
    void Start() {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.CompareTag("Player") && manager.score >= 10) {
            // Dahik pls
        }
    }
}
