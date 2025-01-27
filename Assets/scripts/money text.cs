using UnityEngine;
using TMPro; // Import TextMesh Pro namespace

public class MoneyDisplay : MonoBehaviour
{
    // The money variable (starting at 0)
    public int money = 0;

    // Reference to the TextMesh Pro UI Text component (TMPro) for displaying money
    public TextMeshProUGUI moneyDisplayText;

    // Update the display text at the start
    void Start()
    {
        // Initialize the text displays
        UpdateMoneyDisplay();
    }

    // Update the money display text with the current money value
    void UpdateMoneyDisplay()
    {
        if (moneyDisplayText != null)
        {
            moneyDisplayText.text = money.ToString();
        }
    }

    // Optional: Method to add money and update the display
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyDisplay(); // Update the money display after adding money
    }

    // Optional: Method to subtract money and update the display
    public void SubtractMoney(int amount)
    {
        money -= amount;
        UpdateMoneyDisplay(); // Update the money display after subtracting money
    }
}
