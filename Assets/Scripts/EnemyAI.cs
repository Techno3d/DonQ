using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    List<Transform> waypoints;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float detectionDistance = 10f;
    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    public float projectileSpeed = 10f;
    public float reloadTime = 1f;
    int pointIndex = 0;
    Vector3 vel = Vector3.zero;
    float WaitTime = 0f;
    GameObject player;
    private AIState aiState = AIState.Patrol;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (aiState) {
            case AIState.Patrol:
                patrol();
                CheckPatrolState();
                break;
            case AIState.Chase:
                Chase();
                CheckChaseState();
                break;
            case AIState.Attack:
                WaitTime += Time.deltaTime;
                if(WaitTime > reloadTime) {
                    Attack();            
                    WaitTime = 0f;
                }
                CheckAttackState();
                break;
        }
    }

    private void CheckAttackState() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool inSight = false;
        if(dist < detectionDistance) {
            RaycastHit hit;
            inSight = Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, detectionDistance) && hit.collider.CompareTag("Player");
            if(inSight && dist < detectionDistance/2) {
                aiState = AIState.Attack;
            }
            else if (inSight)
            {
                aiState = AIState.Chase;
            }
        } else {
            aiState = AIState.Patrol;
        }
    }

    private void CheckChaseState() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool inSight = false;
        if(dist < detectionDistance) {
            RaycastHit hit;
            inSight = Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, detectionDistance) && hit.collider.CompareTag("Player");
            if(inSight && dist < detectionDistance/2) {
                aiState = AIState.Attack;
            }
            else if (inSight)
            {
                aiState = AIState.Chase;
            }
        } else {
            aiState = AIState.Patrol;
        }
    }

    private void CheckPatrolState() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool inSight = false;
        if(dist < detectionDistance) {
            RaycastHit hit;
            inSight = Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, detectionDistance) && hit.collider.CompareTag("Player");
            if(inSight && dist < detectionDistance/2) {
                aiState = AIState.Attack;
            }
            else if (inSight)
            {
                aiState = AIState.Chase;
            }
        } else {
            aiState = AIState.Patrol;
        }
    }

    void patrol() {
        Vector3 dir = (waypoints[pointIndex].transform.position - transform.position).normalized;
        if(Physics.Raycast(transform.position, dir, 4f)) {
            dir = Vector3.Cross(dir, Vector3.up);
        }
        transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(dir.x,dir.z), 0f);
        vel.x = Mathf.MoveTowards(vel.x, dir.x*speed, 20f*Time.deltaTime);
        vel.z = Mathf.MoveTowards(vel.z, dir.z*speed, 20f*Time.deltaTime);

        if((waypoints[pointIndex].transform.position - transform.position).magnitude < 1.0f) {
            pointIndex = (pointIndex+1)%waypoints.Count;
        }
        GetComponent<Rigidbody>().velocity = vel;
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.CompareTag("Wall")) {
            pointIndex = (pointIndex+1)%waypoints.Count;
        }
    }

    void Chase() {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 dir = new Vector3(
            playerPos.x - transform.position.x,
            0f,
            playerPos.z - transform.position.z 
        );
        dir.Normalize();
        dir.y = transform.position.y;
        transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(dir.x,dir.z), 0f);
        vel.x = Mathf.MoveTowards(vel.x, dir.x*speed, 20f*Time.deltaTime);
        vel.z = Mathf.MoveTowards(vel.z, dir.z*speed, 20f*Time.deltaTime);
        GetComponent<Rigidbody>().velocity = vel;
    }

    void Attack() {
        Vector3 bulletVel = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized * projectileSpeed;
        // Instantiate and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position + bulletVel*0.1f, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = bulletVel;
        GetComponent<Rigidbody>().velocity = -bulletVel;
    }
}

enum AIState {
    Patrol, Chase, Attack
}