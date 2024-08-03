using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    private Player player;

    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    

    IEnumerator FindPlayerCoroutine()
    {
        while (player == null)
        {
            player = FindObjectOfType<Player>();
            yield return new WaitForSeconds(0.1f);
        }
        InitializeHealthBar();
    }

    void InitializeHealthBar()
    {
        if (healthSlider != null && player != null)
        {
            healthSlider.maxValue = player.maxHealth;
            healthSlider.value = player.HP;
        }
    }

    void Update()
    {
        if (player != null && healthSlider != null)
        {
            healthSlider.value = player.HP;
            healthSlider.maxValue = player.maxHealth;
        }
    }
}