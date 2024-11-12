using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    [SerializeField]
    Transform player;
    GameManager manager;
    void Start() {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update() {
        if(Vector3.Distance(player.transform.position, transform.position) < 30 && manager.score >= 10) {
           Cursor.lockState = CursorLockMode.None;
           SceneManager.LoadScene("Win");
        }
        Debug.Log(Vector3.Distance(player.transform.position, transform.position) < 30 && manager.score >= 10);
    }
}
