using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public float initialHeight = 0.5f;
    public TreasureSpawner parent;
    public int index;
    void Start()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            Debug.Log("HELPPP");
            initialHeight = hit.point.y;
        }
        transform.position = new Vector3(transform.position.x, initialHeight, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().IncScore(1);
            parent.treasureList.RemoveAt(index);
            Destroy(gameObject); 
        }
    }
}
