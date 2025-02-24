using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMesh Pro namespace

public class ShootingAiManager : MonoBehaviour
{
    // Singleton instance
    public static ShootingAiManager Instance { get; private set; }

    // Static variable to track total atom parts across all instances
    public static int totalAtomParts = 0;

    // Reference to a TextMeshPro object to display the total parts
    [SerializeField]
    private TextMeshProUGUI partsText;

    // Ensure there is only one instance of ShootingAiManager
    private void Awake()
    {
        // Check if there's already an instance of this manager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }

        Instance = this; // Set the singleton instance
        DontDestroyOnLoad(gameObject); // Optionally, you can keep this instance across scenes if desired
    }

    // Method to add parts to the total and update the display
    public void AddParts(int partsToAdd)
    {
        // Add the specified number of parts to the total atom parts count
        totalAtomParts += partsToAdd;

        // Update the UI text to display the new total
        UpdatePartsDisplay();

        // Log the total atom parts count (for debugging)
        Debug.Log("Total Atom Parts: " + totalAtomParts);
    }

    // Method to update the TextMeshPro display
    private void UpdatePartsDisplay()
    {
        if (partsText != null)
        {
            // Convert the totalAtomParts (int) to a string before assigning it to text
            partsText.text = totalAtomParts.ToString();
        }
        else
        {
            Debug.LogWarning("Parts TextMeshPro reference is not set in the Inspector!");
        }
    }


    // Optionally, you can manually call UpdatePartsDisplay in other places to ensure real-time updates
    void Update()
    {
        UpdatePartsDisplay();  // Keep updating the display each frame to match totalAtomParts
    }
}
