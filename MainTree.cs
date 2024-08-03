using UnityEngine;
using UnityEngine.UI;

public class MainTree : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] private Slider treeHpSlider;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthSlider();
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            GameManager.instance.SetGameOver();
            Debug.Log("Tree is destroyed");
        }
    }

    private void UpdateHealthSlider()
    {
        if (treeHpSlider != null)
        {
            treeHpSlider.value = (float)currentHealth / maxHealth * 100;
            Debug.Log("Slider value updated to: " + treeHpSlider.value);
        }
        else
        {
            Debug.LogError("Tree HP Slider is not assigned!");
        }
    }

   
    public void UpdateHealthBar(float value)
    {
        Debug.Log("UpdateHealthBar called with value: " + value);
    }
}