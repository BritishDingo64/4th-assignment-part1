using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMesh Pro
using UnityEngine.SceneManagement; // Make sure to import SceneManagement

public class UIManager : MonoBehaviour
{
    public GunSystem gunSystem;  // Reference to the GunSystem
    public MoneyDisplay moneyDisplay;  // Reference to the MoneyDisplay system
    public AudioManager audioManager;  // Reference to the AudioManager

    // Buttons for each stat change
    public Button damageIncreaseButton;
    public Button speedIncreaseButton;
    public Button rangeIncreaseButton;
    public Button bulletsIncreaseButton;
    public Button allowHoldButton;
    public Button unlockRadiationZoneButton;  // New button for unlocking radiation zone

    // Slider for spread control (no longer connected to purchase)
    public Slider spreadSlider;

    // TextMesh Pro Text elements to display purchase counts and costs
    public TMP_Text damagePurchaseText;
    public TMP_Text speedPurchaseText;
    public TMP_Text rangePurchaseText;
    public TMP_Text bulletsPurchaseText;
    public TMP_Text purchaseAllowHoldText;
    public TMP_Text purchaseRadiationZoneText;  // Text for the radiation zone button

    // Values for how much to change each stat by
    public int damageChangeAmount = 5;
    public float speedChangeAmount = 0.1f;
    public float rangeChangeAmount = 5f;
    public int bulletsChangeAmount = 1;

    // Define max caps
    private const int maxDamage = 20;
    private const float maxShootingSpeed = 0.1f; // Time between shots (the lower, the faster)
    private const int maxBullets = 11;

    // Price and price increase percentage for each stat
    public int initialDamagePrice = 50;
    public int initialSpeedPrice = 40;
    public int initialRangePrice = 60;
    public int initialBulletsPrice = 30;
    public float priceIncreasePercentage = 0.4f; // 40% increase after each purchase

    // Price for the "Allow Hold" toggle (you can set this to any value you want)
    public int allowHoldPrice = 200;

    // Reference to the Radiation Zone Collider
    public Collider radiationZoneCollider; // Set in the Inspector

    private bool radiationZoneUnlocked = false;  // Flag to check if the radiation zone is unlocked

    private void Start()
    {
        // Ensure buttons are hooked up in the inspector
        if (damageIncreaseButton != null)
            damageIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Damage"); audioManager.PlayUISound(); });

        if (speedIncreaseButton != null)
            speedIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Speed"); audioManager.PlayUISound(); });

        if (rangeIncreaseButton != null)
            rangeIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Range"); audioManager.PlayUISound(); });

        if (bulletsIncreaseButton != null)
            bulletsIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Bullets"); audioManager.PlayUISound(); });

        if (allowHoldButton != null)
            allowHoldButton.onClick.AddListener(() => { ToggleAllowHold(); audioManager.PlayUISound(); });

        // Set up the spread slider
        if (spreadSlider != null)
        {
            spreadSlider.onValueChanged.AddListener(OnSpreadSliderChanged);
            spreadSlider.value = gunSystem.spread;  // Initialize slider value
        }

        // Setup the Unlock Radiation Zone Button
        if (unlockRadiationZoneButton != null)
            unlockRadiationZoneButton.onClick.AddListener(() => { UnlockRadiationZone(); audioManager.PlayUISound(); });

        // Initial update of the purchase text
        UpdatePurchaseText();
    }

    // Called when the spread slider value changes
    private void OnSpreadSliderChanged(float value)
    {
        if (gunSystem != null)
        {
            gunSystem.SetSpread(value);
            // No need to update purchase text for spread since it's free
        }
    }

    // Unlock the Radiation Zone and make the collider a trigger
    private void UnlockRadiationZone()
    {
        // Check if radiation zone is already unlocked
        if (radiationZoneUnlocked) return;

        // Check if player has enough money to unlock the radiation zone (use an appropriate cost here)
        int unlockCost = 100; // You can change this to any price you like
        if (moneyDisplay.money >= unlockCost)
        {
            moneyDisplay.SubtractMoney(unlockCost);  // Deduct money
            radiationZoneCollider.isTrigger = true;  // Set the collider to be a trigger
            radiationZoneUnlocked = true;  // Mark it as unlocked
            UpdatePurchaseText();  // Update UI to reflect the changes
        }
    }

    // Toggle the "Allow Hold" feature and cost money for it
    private void ToggleAllowHold()
    {
        // Check if the player has enough money
        if (moneyDisplay.money >= allowHoldPrice)
        {
            moneyDisplay.SubtractMoney(allowHoldPrice);  // Deduct money
            gunSystem.ToggleAllowButtonHold();  // Toggle the allow hold feature
            UpdatePurchaseText();  // Update UI to reflect the changes
        }
    }

    // Try to purchase an upgrade based on the type
    private void TryPurchaseUpgrade(string upgradeType)
    {
        int price = 0;

        // Determine the price based on the upgrade type
        switch (upgradeType)
        {
            case "Damage":
                price = GetUpgradePrice("Damage");
                if (moneyDisplay.money >= price)
                {
                    gunSystem.ChangeDamage(damageChangeAmount, maxDamage);
                    moneyDisplay.SubtractMoney(price);
                    UpdateUpgradePrice("Damage");
                }
                break;
            case "Speed":
                price = GetUpgradePrice("Speed");
                if (moneyDisplay.money >= price)
                {
                    gunSystem.ChangeTimeBetweenShooting(-speedChangeAmount, maxShootingSpeed);
                    moneyDisplay.SubtractMoney(price);
                    UpdateUpgradePrice("Speed");
                }
                break;
            case "Range":
                price = GetUpgradePrice("Range");
                if (moneyDisplay.money >= price)
                {
                    gunSystem.ChangeRange(rangeChangeAmount);
                    moneyDisplay.SubtractMoney(price);
                    UpdateUpgradePrice("Range");
                }
                break;
            case "Bullets":
                price = GetUpgradePrice("Bullets");
                if (moneyDisplay.money >= price)
                {
                    // Ensure bullets purchase does NOT reset timeBetweenShooting
                    gunSystem.ChangeBulletsPerTap(bulletsChangeAmount, maxBullets);
                    moneyDisplay.SubtractMoney(price);
                    UpdateUpgradePrice("Bullets");
                }
                break;
        }

        UpdatePurchaseText();  // Update UI to reflect changes
    }

    // Get the current price for a specific upgrade
    private int GetUpgradePrice(string upgradeType)
    {
        switch (upgradeType)
        {
            case "Damage": return initialDamagePrice;
            case "Speed": return initialSpeedPrice;
            case "Range": return initialRangePrice;
            case "Bullets": return initialBulletsPrice;
            default: return 0;
        }
    }

    // Update the price for a specific upgrade by the percentage increase
    private void UpdateUpgradePrice(string upgradeType)
    {
        switch (upgradeType)
        {
            case "Damage":
                initialDamagePrice = (int)(initialDamagePrice * (1 + priceIncreasePercentage));
                break;
            case "Speed":
                initialSpeedPrice = (int)(initialSpeedPrice * (1 + priceIncreasePercentage));
                break;
            case "Range":
                initialRangePrice = (int)(initialRangePrice * (1 + priceIncreasePercentage));
                break;
            case "Bullets":
                initialBulletsPrice = (int)(initialBulletsPrice * (1 + priceIncreasePercentage));
                break;
        }
    }

    // Update purchase count texts for each stat and cost for the next purchase
    private void UpdatePurchaseText()
    {
        if (damagePurchaseText != null)
            damagePurchaseText.text = $"Damage: {gunSystem.damagePurchaseCount} | Cost: {GetUpgradePrice("Damage")}";

        if (speedPurchaseText != null)
            speedPurchaseText.text = $"Speed: {gunSystem.speedPurchaseCount} | Cost: {GetUpgradePrice("Speed")}";

        if (rangePurchaseText != null)
            rangePurchaseText.text = $"Range: {gunSystem.rangePurchaseCount} | Cost: {GetUpgradePrice("Range")}";

        if (bulletsPurchaseText != null)
            bulletsPurchaseText.text = $"Bullets: {gunSystem.bulletsPurchaseCount} | Cost: {GetUpgradePrice("Bullets")}";

        // Show how many times 'Allow Hold' has been toggled and the cost for the next toggle
        if (purchaseAllowHoldText != null)
            purchaseAllowHoldText.text = $"Allow Hold Toggled: {(gunSystem.allowButtonHold ? "1" : "0")} | Cost: {allowHoldPrice}";

        // Show if the radiation zone is unlocked
        if (purchaseRadiationZoneText != null)
            purchaseRadiationZoneText.text = radiationZoneUnlocked ? "Radiation Zone Unlocked" : "Radiation Zone Locked";
    }
}