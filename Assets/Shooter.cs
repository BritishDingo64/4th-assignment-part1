using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int bulletsPerTap = 1; // Number of bullets fired per tap
    public bool allowButtonHold;

    // Bullet settings
    public GameObject bulletPrefab;  // The prefab of the bullet that will be shot
    public float bulletSpeed = 10f;  // Speed of the bullet

    // Recoil settings (optional)
    public float recoilForce = 5f;  // Amount of recoil when the gun is fired
    private Rigidbody shipRb;       // Rigidbody for the ship to apply recoil

    // Bools
    private bool shooting, readyToShoot, reloading;

    // References
    public Transform attackPoint;  // This is the position of the gun
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy; // LayerMask for enemy layers

    // Audio Manager reference
    public AudioManager audioManager;

    private void Awake()
    {
        readyToShoot = true;
        shipRb = GetComponent<Rigidbody>();  // Get the ship's Rigidbody component for recoil
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (shooting && readyToShoot && !reloading)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Fire the bullets in the direction the ship is facing (based on the gun's rotation)
        for (int i = 0; i < bulletsPerTap; i++)
        {
            // Fire direction with spread
            Vector3 direction = attackPoint.forward;  // This makes sure the bullet fires in the direction the gun is facing

            // Apply spread to the direction of the shot
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            direction += new Vector3(x, y, 0);  // Add spread in X and Y directions

            // Instantiate the bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);

            // Give the bullet the calculated direction and speed
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply velocity in the direction the gun is facing, with spread and speed
                rb.linearVelocity = direction.normalized * bulletSpeed;
            }

            // Set a lifetime for the bullet so it doesn’t travel forever
            Destroy(bullet, range / bulletSpeed); // Bullet lifetime is based on the range and speed

            // RayCast for shooting - Ignore the "Player" layer
            if (Physics.Raycast(attackPoint.position, direction, out rayHit, range, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
            {
                // Process hit based on object types
                Debug.Log(rayHit.collider.name);

                if (rayHit.collider.CompareTag("Enemy"))
                {
                    rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
                }
                else if (rayHit.collider.CompareTag("Map"))
                {
                    // Optionally: You can instantiate a bullet hole graphic here if needed
                }
            }

            // Apply recoil force to the ship
            if (shipRb != null)
            {
                shipRb.AddForce(-attackPoint.forward * recoilForce, ForceMode.Impulse);  // Recoil in the opposite direction of the shot
            }

            // Optional delay between bullets if firing multiple shots per tap
            if (i < bulletsPerTap - 1)
            {
                float delay = i * timeBetweenShots;
                Invoke("ResetShot", delay);
            }
        }

        // Play the gun fire sound
        if (audioManager != null)
        {
            Debug.Log("Playing Gun Fire Sound");
            audioManager.PlayGunFire();
        }

        // Reset shot and prevent multiple firing
        Invoke("ResetShot", timeBetweenShooting);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
}
