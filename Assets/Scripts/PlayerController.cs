using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float cooldown = 0.25f;
    private float lastActionTime = 0f;
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
    private Animator animator;
    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            mouseLocked = !mouseLocked;
            Cursor.lockState = mouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedmod = 3f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedmod = 1.8f;
        }
        // Input
        float xaxis = -Input.GetAxis("Vertical");
        float zaxis = Input.GetAxis("Horizontal");
        float mousex = Input.GetAxis("Mouse X");
        rotY += mousex * rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotY * Mathf.Rad2Deg, 0);
        // I love matrix multiplication
        Vector3 direction = new Vector3(
            zaxis * Mathf.Cos(rotY) - xaxis * Mathf.Sin(rotY),
            0f,
            -zaxis * Mathf.Sin(rotY) - xaxis * Mathf.Cos(rotY)
        );

        direction = Vector3.ClampMagnitude(direction, 1f);
        if (direction == Vector3.zero)
        {
            animator.SetFloat("Speed", 0f);
        }
        else if (speedmod == 2f)
        {
            animator.SetFloat("Speed", 1.5f);
        }
        else
        {
            animator.SetFloat("Speed", 1f);
        }
        velocity.x = Mathf.MoveTowards(velocity.x, direction.x * 5f * speedmod, 30f * Time.deltaTime);
        velocity.z = Mathf.MoveTowards(velocity.z, direction.z * 5f * speedmod, 30f * Time.deltaTime);

        // Apply gravity
        if (!controller.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
        // Jumping
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity) * Mathf.Sqrt(speedmod) * 0.6f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time >= lastActionTime + cooldown)
            {

                Attack();
                lastActionTime = Time.time;  // Update last action time
            }

        }
        if (health.health <= 0)
        {
            ///Death logic for dahik!!!! mwah <3 u my goat dahik ♥
        }
    }

    // Called via animation event
    public void Attack()
    {
        // Instantiate and shoot the projectile
        Vector3 bulletVel = cam.transform.forward * projectileSpeed;
        GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 1.8f + bulletVel * 0.15f, Quaternion.identity);
        projectile.GetComponent<Projectile>().funnyTag = "Enemy";
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = bulletVel * speedmod;
    }
}
