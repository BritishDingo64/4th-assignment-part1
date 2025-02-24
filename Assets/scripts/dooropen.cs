using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioManager audioManager; // Reference to the AudioManager script

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the collider
        if (other.CompareTag("Player"))
        {
            // Call the PlayDoorPhase function from the AudioManager
            audioManager.PlayDoorPhase();
        }
    }
}
