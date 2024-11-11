using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasurePrefab;
    public List<GameObject> nodes;
    public float radius = 25f;
    public List<GameObject> treasureList;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject node in nodes) {
            Vector3 refNode = node.transform.position;
            Vector3 spawn = new Vector3(
                Random.Range(refNode.x + radius*-5, refNode.x + radius*5),
                200f,
                Random.Range(refNode.z + radius*-5, refNode.z + radius*5)
            );
            treasureList.Add(Instantiate(treasurePrefab, spawn, Quaternion.identity, transform));
        }
    }
}
