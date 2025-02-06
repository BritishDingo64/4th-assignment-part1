using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMesh Pro

public class UIManager : MonoBehaviour
{
    public GunSystem gunSystem;  // Reference to the GunSystem
    public MoneyDisplay moneyDisplay;  // Reference to the MoneyDisplay system

    // Buttons for each stat change
    public Button damageIncreaseButton;
    public Button speedIncreaseButton;
    public Button rangeIncreaseButton;
    public Button bulletsIncreaseButton;
    public Button allowHoldButton;

    // Slider for spread control (no longer connected to purchase)
    public Slider spreadSlider;

    // TextMesh Pro Text elements to display purchase counts
    public TMP_Text damagePurchaseText;
    public TMP_Text speedPurchaseText;
    public TMP_Text rangePurchaseText;
    public TMP_Text bulletsPurchaseText;
    public TMP_Text purchaseAllowHoldText;

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
    public float priceIncreasePercentage = 0.1f; // 10% increase after each purchase

    private void Start()
    {
        // Ensure buttons are hooked up in the inspector
        if (damageIncreaseButton != null)
            damageIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Damage"); });

        if (speedIncreaseButton != null)
            speedIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Speed"); });

        if (rangeIncreaseButton != null)
            rangeIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Range"); });

        if (bulletsIncreaseButton != null)
            bulletsIncreaseButton.onClick.AddListener(() => { TryPurchaseUpgrade("Bullets"); });

        if (allowHoldButton != null)
            allowHoldButton.onClick.AddListener(() => { gunSystem.ToggleAllowButtonHold(); UpdatePurchaseText(); });

        // Set up the spread slider
        if (spreadSlider != null)
        {
            spreadSlider.onValueChanged.AddListener(OnSpreadSliderChanged);
            spreadSlider.value = gunSystem.spread;  // Initialize slider value
        }
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

    // Update purchase count texts for each stat
    private void UpdatePurchaseText()
    {
        if (damagePurchaseText != null) damagePurchaseText.text = "Damage Purchases: " + gunSystem.damagePurchaseCount;
        if (speedPurchaseText != null) speedPurchaseText.text = "Speed Purchases: " + gunSystem.speedPurchaseCount;
        if (rangePurchaseText != null) rangePurchaseText.text = "Range Purchases: " + gunSystem.rangePurchaseCount;
        if (bulletsPurchaseText != null) bulletsPurchaseText.text = "Bullets Purchases: " + gunSystem.bulletsPurchaseCount;

        // Show how many times 'Allow Hold' has been toggled
        if (purchaseAllowHoldText != null)
            purchaseAllowHoldText.text = "Allow Hold Toggled: " + (gunSystem.allowButtonHold ? "1" : "0");
    }
}
