using UnityEngine;

public class PlayerMouseRotation : MonoBehaviour
{
    public Camera playerCamera;  // Reference to the camera
    public float rotationSpeed = 10f;  // Speed of rotation

    void Start()
    {
        // If playerCamera is not set in the inspector, try to assign Camera.main
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Check if we still don't have a camera
        if (playerCamera == null)
        {
            Debug.LogError("No camera found. Please assign a camera to the playerCamera variable.");
        }
    }

    void Update()
    {
        if (playerCamera == null)
            return;

        // Step 1: Get the mouse position in world space
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Step 2: Get the direction from the player to the mouse's world position
            Vector3 targetPosition = hit.point;

            // Step 3: Calculate the rotation around the Y-axis (horizontal rotation)
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;  // Ignore vertical rotation

            // Step 4: Create a rotation to look at the target position horizontally
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Step 5: Apply rotation smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
