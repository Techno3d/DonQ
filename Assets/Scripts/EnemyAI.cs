using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
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
    float attackDist = 4f;
    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    public float projectileSpeed = 10f;
    GameObject treasureSpawner;
    public float reloadTime = 1f;
    int pointIndex = 0;
    Vector3 vel = Vector3.zero;
    Vector3 assumedLocation = Vector3.zero;
    float WaitTime = 0;
    GameObject player;
    private AIState aiState = AIState.Patrol;
    // Start is called before the first frame update
    void Start()
    {
        treasureSpawner = GameObject.FindGameObjectWithTag("TreasureSpawner");
        player = GameObject.FindGameObjectWithTag("Player");
        WaitTime = reloadTime;
    }

    // Update is called once per frame
    void Update() {
        // Todo - Remove later
        if(GetComponent<Health>().health <= 0) {
            Destroy(gameObject);
            return;
        }
        switch (aiState) {
            case AIState.Patrol:
                Patrol();
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
            case AIState.Find:
                WaitTime += Time.deltaTime;
                Find();
                CheckFindState();
                break;
            case AIState.Hunt:
                Chase();
                CheckHuntState();
                break;
        }
    }

    private void CheckHuntState() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        bool inSight = Physics.Raycast(transform.position, (player.transform.position + Vector3.up - transform.position).normalized, out RaycastHit hit, detectionDistance) && hit.collider.CompareTag("Player");
        if (inSight && dist < attackDist) { // Hunt -> Attack
            aiState = AIState.Attack;
        }
    }

    private void CheckFindState() {
        if(treasureSpawner.GetComponent<TreasureSpawner>().treasureList.Count <= 0) {
            aiState = AIState.Hunt;
            return;
        }
        // If its been trying for too long, go back to patrol
        if(WaitTime > 2f) {
            aiState = AIState.Patrol;
            return;
        }

        // If not, see if player is in sight
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist > detectionDistance) return; // Don't raycast if its not even possible to see
        bool inSight = Physics.Raycast(transform.position, (player.transform.position + Vector3.up - transform.position).normalized, out RaycastHit hit, detectionDistance) && hit.collider.CompareTag("Player");
        if (inSight && dist < attackDist) { // Stay Attacking
            aiState = AIState.Attack;
        }
        else if (inSight) // Attack -> Chase
        {
            aiState = AIState.Chase;
        }
    }

    private void CheckAttackState() {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist > detectionDistance) { // Attack -> Patrol
            if(treasureSpawner.GetComponent<TreasureSpawner>().treasureList.Count <= 0) {
                aiState = AIState.Hunt;
                return;
            }
            assumedLocation = transform.position + (player.transform.position + Vector3.up - transform.position).normalized*2f;
            aiState = AIState.Find;
            return;
        }
        bool inSight = Physics.Raycast(transform.position, (player.transform.position + Vector3.up - transform.position).normalized, out RaycastHit hit, detectionDistance) && hit.collider.CompareTag("Player");
        if (inSight && dist < attackDist) { // Stay Attacking
            aiState = AIState.Attack;
        }
        else if (inSight) // Attack -> Chase
        {
            aiState = AIState.Chase;
        }
    }

    // Similar to CheckAttackState
    private void CheckChaseState() {
        if(treasureSpawner.GetComponent<TreasureSpawner>().treasureList.Count <= 0) {
            aiState = AIState.Hunt;
            return;
        }
        float dist = Vector3.Distance(player.transform.position, transform.position);
        // Player leaves distance
        if(dist > detectionDistance) { // Chase -> Patrol
            assumedLocation = transform.position + (player.transform.position + Vector3.up - transform.position).normalized*2f;
            aiState = AIState.Find;
            return;
        }
        // Check for sight (walls, etc)
        bool inSight = Physics.Raycast(transform.position, (player.transform.position + Vector3.up - transform.position).normalized, out RaycastHit hit, detectionDistance) && hit.collider.CompareTag("Player");
        if (inSight && dist < attackDist) { // Chase -> Attack
            aiState = AIState.Attack;
        }
        else if (inSight) // Stay Chasing
        {
            aiState = AIState.Chase;
        }
    }

    private void CheckPatrolState() {
        if(treasureSpawner.GetComponent<TreasureSpawner>().treasureList.Count <= 0) {
            aiState = AIState.Hunt;
            return;
        }
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist > detectionDistance) { // Attack -> Patrol
            aiState = AIState.Patrol;
            return;
        }
        bool inSight = Physics.Raycast(transform.position, (player.transform.position + Vector3.up - transform.position).normalized, out RaycastHit hit, detectionDistance) && hit.collider.CompareTag("Player");
        if (inSight) // Attack -> Chase
        {
            aiState = AIState.Chase;
        }
    }

    void Patrol() {
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
    
    void Find() {
        Vector3 dir = (assumedLocation - transform.position).normalized;
        if(Physics.Raycast(transform.position, dir, 4f)) {
            dir = Vector3.Cross(dir, Vector3.up);
        }
        transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(dir.x,dir.z), 0f);
        vel.x = Mathf.MoveTowards(vel.x, dir.x*speed, 20f*Time.deltaTime);
        vel.z = Mathf.MoveTowards(vel.z, dir.z*speed, 20f*Time.deltaTime);
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
        if(!projectile.GetComponent<Projectile>().isAnchored) {
            projectile.transform.position = new Vector3(projectile.transform.position.x, transform.position.y + 2, transform.position.z);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = bulletVel;
            GetComponent<Rigidbody>().velocity = -bulletVel/10f;
        } else {
            projectile.transform.parent = transform;
            projectile.transform.position -= bulletVel*0.1f;
            
        }
    }
}

enum AIState {
    Patrol, Chase, Attack, Find, Hunt
}