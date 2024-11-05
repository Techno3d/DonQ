using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 10;
    public Slider healthBar;
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            Debug.Log("has died!");
            // Implement player death logic, like restarting the game or showing a game over screen.
        }
    }
}