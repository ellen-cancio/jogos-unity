using UnityEngine;
using UnityEngine.UI; // Required for UI elements

// 0 references | Unity Script (1 asset reference)
public class HealthBar : MonoBehaviour
{
    // 2 references
    private Image healthBarFill;

    // 0 references | Unity Message
    void Start()
    {
        healthBarFill = this.GetComponent<Image>();
    }

    // 0 references
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Calculate the percentage (0.0 to 1.0) and assign it to fillAmount
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}