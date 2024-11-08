using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    public float projectileSpeed = 15f;
    public float reloadTime = 1f;

    [SerializeField]
    float rotationSpeed = 360f;
    [SerializeField]
    Camera cam;
    bool mouseLocked = true;
    float rotY = 0;
    CharacterController controller;
    Vector3 velocity = Vector3.zero;

    public float speedmod = 1.3f;
    public float speed = 7.0f;
    public float jumpHeight = 1.0f;
    public float gravity = 9.81f;
    public float turnSmoothTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            mouseLocked = !mouseLocked;
            Cursor.lockState = mouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedmod = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speedmod = 1.3f;
        }
        // Input
        float xaxis = -Input.GetAxis("Vertical");
        float zaxis = Input.GetAxis("Horizontal");
        float mousex = Input.GetAxis("Mouse X");
        rotY += mousex*rotationSpeed*Mathf.Deg2Rad*Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotY*Mathf.Rad2Deg, transform.rotation.z);
        // I love matrix multiplication
        Vector3 direction = new Vector3(
            zaxis*Mathf.Cos(rotY)-xaxis*Mathf.Sin(rotY),
            0f,
            -zaxis*Mathf.Sin(rotY)-xaxis*Mathf.Cos(rotY)
        );
        
        direction = Vector3.ClampMagnitude(direction, 1f);
        velocity.x = Mathf.MoveTowards(velocity.x, direction.x*5f*speedmod, 30f*Time.deltaTime);
        velocity.z = Mathf.MoveTowards(velocity.z, direction.z*5f*speedmod, 30f*Time.deltaTime);
        
        // Apply gravity
        if(!controller.isGrounded) {
            velocity.y -= gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
        // Jumping
        if (controller.isGrounded && Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity) * speedmod;
        }
        if(Input.GetMouseButtonUp(0)) {
            Attack();
        }
    }

    private void Attack() {
        // Instantiate and shoot the projectile
        Vector3 bulletVel = cam.transform.forward * projectileSpeed;
        GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up*1.8f + bulletVel*0.1f, Quaternion.identity);
        projectile.GetComponent<Projectile>().funnyTag = "Enemy";
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = bulletVel*speedmod;
    }
}
