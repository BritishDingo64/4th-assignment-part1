using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;
    public float gravity = -9.8f;
    public Transform cameraTransform;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    private Rigidbody rb;

    // Reference to AudioManager for footsteps sound
    private AudioManager audioManager;

    // Footstep sound trigger variables
    public float footstepInterval = 0.5f;  // Time interval between footstep sounds
    private float footstepTimer = 0f;  // Timer to track time between footstep sounds
    private bool isMoving = false;    // Flag to track whether the player is moving

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Get the AudioManager reference from the scene
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraFollow();
        HandleFootsteps();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();
        Vector3 moveDirection = forward * vertical + right * horizontal;

        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 targetVelocity = moveDirection * moveSpeed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 10f);

            // Player is moving
            isMoving = true;
        }
        else
        {
            // Player is not moving
            isMoving = false;
        }
    }

    private void HandleCameraFollow()
    {
        if (cameraTransform != null)
        {
            Vector3 desiredCameraPosition = transform.position - cameraTransform.forward * cameraDistance + Vector3.up * cameraHeight;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredCameraPosition, Time.deltaTime * 10f);
            cameraTransform.LookAt(transform);
        }
    }

    private void HandleFootsteps()
    {
        // If the player is moving and footstep interval has passed
        if (isMoving)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0f;

                // Ensure AudioManager is available and play walking sound
                if (audioManager != null)
                {
                    audioManager.PlayWalking();  // Play the walking sound
                }
            }
        }
        else
        {
            // If the player stops moving, stop the footstep sound immediately
            if (audioManager != null)
            {
                // Stop the walking sound immediately when the player stops moving
                audioManager.StopWalking();
            }

            // Reset footstep timer if not moving
            footstepTimer = 0f;
        }
    }
}
