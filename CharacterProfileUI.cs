using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterProfileUI : MonoBehaviour
{
    public static CharacterProfileUI Instance;

    public Image characterImage;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI moveSpeedText;

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

    public void UpdateProfile(Player player, Weapon weapon)
    {
        if (player != null)
        {
            if (maxHealthText != null) maxHealthText.text = $"Max Health: {player.maxHealth}";
            if (moveSpeedText != null) moveSpeedText.text = $"Move Speed: {player.moveSpeed}";
        }

        if (weapon != null)
        {
            if (damageText != null) damageText.text = $"Damage: {weapon.damage}";
        }
    }
}