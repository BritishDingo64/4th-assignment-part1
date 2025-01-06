using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public AudioManager audioManager; // Reference to the AudioManager to play sound
    public TextMeshProUGUI doorText;  // UI text to display door interaction message
    public float openAngle = 90f;     // Rotation angle for opening the door
    public float speed = 2f;          // Speed of the door opening

    private bool isOpen = false;      // Whether the door is open
    private bool playerNearby = false;// If the player is near the door

    private Quaternion initialRotation; // Starting rotation of the door
    private Quaternion targetRotation;  // Target rotation when the door is open

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
        doorText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check if the player is near and if the "E" key is pressed
        if (playerNearby)
        {
            doorText.gameObject.SetActive(true);
            doorText.text = "Press E to Interact With Door";

            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;

                if (isOpen)
                {
                    OpenDoor();
                }
                else
                {
                    CloseDoor();
                }
            }
        }

        // Smoothly rotate the door to the open or closed position
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            isOpen ? targetRotation : initialRotation,
            Time.deltaTime * speed
        );
    }

    // Open the door and play the sound effect
    public void OpenDoor()
    {
        isOpen = true;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * speed
        );

        // Play the door open sound effect
        if (audioManager != null)
        {
            audioManager.PlayDoorOpen();  // Call the method to play sound
        }
    }

    // Close the door (if necessary)
    public void CloseDoor()
    {
        isOpen = false;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            initialRotation,
            Time.deltaTime * speed
        );
    }

    // When the player enters the trigger collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    // When the player exits the trigger collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            doorText.gameObject.SetActive(false); // Hide door interaction text
        }
    }
}
