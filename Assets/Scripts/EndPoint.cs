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

    void Update() {
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist < 5 && manager.score >= 10) {
            //Transition to win scene
        }
    }
}