using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAi : MonoBehaviour
{
    private int health = 100;

    // Reference to a death effect
    public GameObject deathEffect;

    // Method to apply damage to the ATOM
    public void TakeDamage(int damage)
    {
        health -= damage; // Subtracting damage from health

        // Log the current health to debug
        Debug.Log("atom Health: " + health);

        if (health <= 0) // If health reaches 0 or below, destroy the enemy
        {
            Die();
        }
    }


    // Handle the destruction of the atom
    private void Die()
    {
        // Play a destroy effect
        if (deathEffect != null)
        {
            // Instantiate the destruction effect and destroy it after 2 seconds
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // Log that the atom has been smashed
        Debug.Log("atom Died!");

        // Destroy the object
        Destroy(gameObject);
    }
}
