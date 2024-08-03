using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    public Player playerScript;
    [SerializeField] private Image ToUpgradeHealthImage;
    [SerializeField] private Image ToUpgradeDamageImage;
    [SerializeField] private Image ToUpgradeMovespeedImage;
    [SerializeField] private Sprite copperCoinSprite;
    [SerializeField] private Sprite silverCoinSprite;
    [SerializeField] private Sprite goldCoinSprite;
    
    public Weapon weaponScript;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI healthBarValueText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI CoinRequiredToUpgradeHealth;
    public TextMeshProUGUI CoinRequiredToUpgradeDamage;
    public TextMeshProUGUI CoinRequiredToUpgradeMovespeed;
    private int[] hpUpgradeCost = { 10, 20, 5, 10, 20, 1 };
    private int[] damageUpgradeCost = { 10, 20, 5, 10, 20, 1 };
    private int[] speedUpgradeCost = { 10, 20, 5, 10, 20, 1 };
    private int hpUpgradeLevel = 0;
    private int damageUpgradeLevel = 0;
    private int speedUpgradeLevel = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerScript = FindObjectOfType<Player>();
    }
    private bool CanAffordUpgrade(int[] costs, int level)
{
    int index = level % 6;
    if (index < 2)
        return GameManager.instance.coinData.copper >= costs[index];
    else if (index < 4)
        return GameManager.instance.coinData.silver >= costs[index];
    else
        return GameManager.instance.coinData.gold >= costs[index];
}

private void DeductCoins(int[] costs, int level)
{
    int index = level % 6;
    if (index < 2)
        GameManager.instance.coinData.copper -= costs[index];
    else if (index < 4)
        GameManager.instance.coinData.silver -= costs[index];
    else
        GameManager.instance.coinData.gold -= costs[index];
    
    GameManager.instance.UpdateCoinUI();
}



     public void UpgradeHealth()
    {
        if (playerScript != null && CanAffordUpgrade(hpUpgradeCost, hpUpgradeLevel))
        {
            playerScript.maxHealth += 50; // or whatever amount you want to increase
            playerScript.hp = playerScript.maxHealth;
            playerScript.UpdateHealthSlider();
            maxHealthText.text = playerScript.maxHealth.ToString();
            healthBarValueText.text = $"{playerScript.hp} / {playerScript.maxHealth}";
            
            DeductCoins(hpUpgradeCost, hpUpgradeLevel);
            hpUpgradeLevel++;
            CoinRequiredToUpgradeHealth.text ="x" + hpUpgradeCost[hpUpgradeLevel % 6].ToString();
            UpdateHealthCoinImage();
            Debug.Log("Health upgraded successfully!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade health!");
        }
    }
    private void UpdateHealthCoinImage()
{
    if (ToUpgradeHealthImage != null)
    {
        if (hpUpgradeLevel < 2)
            ToUpgradeHealthImage.sprite = copperCoinSprite;
        else if (hpUpgradeLevel < 4)
            ToUpgradeHealthImage.sprite = silverCoinSprite;
        else
            ToUpgradeHealthImage.sprite = goldCoinSprite;
    }
}
 private void UpdateDamageCoinImage()
{
    if (ToUpgradeDamageImage != null)
    {
        if (damageUpgradeLevel < 2)
            ToUpgradeDamageImage.sprite = copperCoinSprite;
        else if (damageUpgradeLevel < 4)
            ToUpgradeDamageImage.sprite = silverCoinSprite;
        else
            ToUpgradeDamageImage.sprite = goldCoinSprite;
    }
}
 private void UpdateMovespeedCoinImage()
{
    if (ToUpgradeMovespeedImage != null)
    {
        if (speedUpgradeLevel < 2)
            ToUpgradeMovespeedImage.sprite = copperCoinSprite;
        else if (speedUpgradeLevel < 4)
            ToUpgradeMovespeedImage.sprite = silverCoinSprite;
        else
            ToUpgradeMovespeedImage.sprite = goldCoinSprite;
    }
}

 
    public void UpdateHealthBarValue() {
        healthBarValueText.text = $"{playerScript.hp} / {playerScript.maxHealth}"; 
    }

    public void setPlayerScript(Player player)
    {
        playerScript = player;
    }
    public void setWeaponScript(Weapon weapon)
    {
        weaponScript = weapon;
    }
    public void UpgradeDamage()
    {
        if (weaponScript != null && CanAffordUpgrade(damageUpgradeCost, damageUpgradeLevel))
        {
            weaponScript.damage += 10; // or whatever amount you want to increase
            damageText.text = weaponScript.damage.ToString();
            
            DeductCoins(damageUpgradeCost, damageUpgradeLevel);
            damageUpgradeLevel++;
            CoinRequiredToUpgradeDamage.text ="x" + damageUpgradeCost[damageUpgradeLevel % 6].ToString();
            
            UpdateDamageCoinImage();
            Debug.Log("Damage upgraded successfully!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade damage!");
        }
    }

     
    public void setDamageToDefault() {
        weaponScript.damage = 10;
        
    }
     public void UpgradeMoveSpeed()
    {
        if (playerScript != null && CanAffordUpgrade(speedUpgradeCost, speedUpgradeLevel))
        {
            playerScript.moveSpeed += 0.5f; // or whatever amount you want to increase
            moveSpeedText.text = playerScript.moveSpeed.ToString();
            
            DeductCoins(speedUpgradeCost, speedUpgradeLevel);
            speedUpgradeLevel++;
            CoinRequiredToUpgradeMovespeed.text ="x" + speedUpgradeCost[speedUpgradeLevel % 6].ToString();
            
            UpdateMovespeedCoinImage();
            Debug.Log("Move speed upgraded successfully!");
        }
        else
        {
            Debug.Log("Not enough coins to upgrade move speed!");
        }
    }

}