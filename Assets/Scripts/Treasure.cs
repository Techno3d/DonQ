using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public float initialHeight = 0.5f;
    void Start()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            initialHeight = 0.5f+hit.point.y;
        }
        transform.position = new Vector3(transform.position.x, initialHeight, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
