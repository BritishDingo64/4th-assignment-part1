using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAiHealth : MonoBehaviour
{
    // Health is now editable in the Inspector
    [SerializeField]
    private int health = 100;

    // Number of parts rewarded upon death, editable in the Inspector
    [SerializeField]
    private int partsOnDeath = 1;

    // Flag to ensure `Die` is only called once
    private bool isDead = false;

    // Method to apply damage to the AI
    public void TakeDamage(int damage)
    {
        if (isDead) return; // If already dead, ignore further damage

        health -= damage; // Subtracting damage from health

        // Log the current health to debug
        Debug.Log("AI Health: " + health);

        if (health <= 0 && !isDead) // If health reaches 0 or below, destroy the AI
        {
            Die();
        }
    }

    // Handle the destruction of the AI
    private void Die()
    {
        if (isDead) return; // Ensure `Die` is not called multiple times

        isDead = true; // Mark as dead

        // Add parts to the total before destroying the object
        if (ShootingAiManager.Instance != null)
        {
            ShootingAiManager.Instance.AddParts(partsOnDeath); // Add parts using the singleton manager
        }

        // Destroy the object
        Destroy(gameObject);
    }

    // Trigger event when another collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the "Bullet" tag
        if (other.CompareTag("Bullet"))
        {
            // Apply damage to the AI when hit by a bullet
            TakeDamage(10); // You can change the damage value as needed

            // Optionally, destroy the bullet after it hits the AI
            Destroy(other.gameObject);

            Debug.Log("Bullet hit the AI!");
        }
    }
}
