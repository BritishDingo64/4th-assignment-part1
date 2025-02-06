using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, timeBetweenShots;
    public int bulletsPerTap = 1; // Number of bullets fired per tap
    public bool allowButtonHold;

    // Bullet settings
    public GameObject bulletPrefab;  // The prefab of the bullet that will be shot
    public float bulletSpeed = 10f;  // Speed of the bullet

    // Recoil settings (optional)
    public float recoilForce = 5f;  // Amount of recoil when the gun is fired
    private Rigidbody shipRb;       // Rigidbody for the ship to apply recoil

    // Bools
    private bool shooting, readyToShoot;

    // References
    public Transform attackPoint;  // This is the position of the gun
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy; // LayerMask for enemy layers

    // Audio Manager reference
    public AudioManager audioManager;

    // Track purchases for each stat
    public int damagePurchaseCount = 0;
    public int speedPurchaseCount = 0;
    public int rangePurchaseCount = 0;
    public int bulletsPurchaseCount = 0;

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

        if (shooting && readyToShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        Invoke("ResetShot", timeBetweenShooting);

        for (int i = 0; i < bulletsPerTap; i++)
        {
            Vector3 direction = attackPoint.forward;
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            direction += new Vector3(x, y, 0);  // Add spread in X and Y directions

            GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * bulletSpeed;
            }

            Destroy(bullet, range / bulletSpeed);

            if (Physics.Raycast(attackPoint.position, direction, out rayHit, range, whatIsEnemy))
            {
                Debug.Log(rayHit.collider.name);
                if (rayHit.collider.CompareTag("Enemy"))
                {
                    ShootingAiHealth enemyHealth = rayHit.collider.GetComponent<ShootingAiHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                    }
                }
            }

            if (shipRb != null)
            {
                shipRb.AddForce(-attackPoint.forward * recoilForce, ForceMode.Impulse);  // Recoil
            }

        }

        if (audioManager != null)
        {
            Debug.Log("Playing Gun Fire Sound");
            audioManager.PlayGunFire();
        }

    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    // Methods to increase stats with max caps
    public void ChangeDamage(int amount, int maxDamage)
    {
        damage += amount;
        damagePurchaseCount++;

        // Apply cap
        if (damage > maxDamage)
            damage = maxDamage;
    }

    public void ChangeTimeBetweenShooting(float amount, float maxTimeBetweenShooting)
    {
        timeBetweenShooting += amount;
        speedPurchaseCount++;

        // Apply cap (the lower, the faster)
        if (timeBetweenShooting < maxTimeBetweenShooting)
            timeBetweenShooting = maxTimeBetweenShooting;
    }

    public void ChangeRange(float amount)
    {
        range += amount;
        rangePurchaseCount++;
    }

    public void ChangeBulletsPerTap(int amount, int maxBullets)
    {
        bulletsPerTap += amount;
        bulletsPurchaseCount++;

        // Apply cap
        if (bulletsPerTap > maxBullets)
            bulletsPerTap = maxBullets;

        // Prevent timeBetweenShooting from being affected
        // We will not modify timeBetweenShooting here!
    }

    public void ToggleAllowButtonHold()
    {
        // Always set to true, so it cannot be toggled off
        allowButtonHold = true;
    }

    // Method to set the spread value
    public void SetSpread(float value)
    {
        spread = value;
    }
}
