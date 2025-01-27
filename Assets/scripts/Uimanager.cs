using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMesh Pro

public class UIManager : MonoBehaviour
{
    public GunSystem gunSystem;  // Reference to the GunSystem

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

    private void Start()
    {
        // Ensure buttons are hooked up in the inspector
        if (damageIncreaseButton != null)
            damageIncreaseButton.onClick.AddListener(() => { gunSystem.ChangeDamage(damageChangeAmount, maxDamage); UpdatePurchaseText(); });

        if (speedIncreaseButton != null)
            speedIncreaseButton.onClick.AddListener(() => { gunSystem.ChangeTimeBetweenShooting(-speedChangeAmount, maxShootingSpeed); UpdatePurchaseText(); });

        if (rangeIncreaseButton != null)
            rangeIncreaseButton.onClick.AddListener(() => { gunSystem.ChangeRange(rangeChangeAmount); UpdatePurchaseText(); });

        if (bulletsIncreaseButton != null)
            bulletsIncreaseButton.onClick.AddListener(() => { gunSystem.ChangeBulletsPerTap(bulletsChangeAmount, maxBullets); UpdatePurchaseText(); });

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
