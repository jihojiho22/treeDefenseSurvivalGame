using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public float minCooldown = 5f;
    public float maxCooldown = 7f;
    private float nextAbilityTime;
    [SerializeField] public Animator anim;
    public Transform player;
    private Enemy enemyScript;

    [SerializeField] private GameObject dangerAreaPrefab;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private int numberOfDangerAreas = 3;
    [SerializeField] private float dangerAreaRadius = 1f;
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float lightningDuration = 5f;

    private List<GameObject> activeDangerAreas = new List<GameObject>();
    private List<GameObject> activeLightning = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyScript = GetComponent<Enemy>();
        SetNextAbilityTime();
    }

    void Update()
    {
        if (Time.time >= nextAbilityTime)
        {
            TriggerAbility();
            SetNextAbilityTime();
        }
        if (player != null)
        {
            // Flip the sprite based on player's position
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(5, 5, 5); // Face left
            }
            else
            {
                transform.localScale = new Vector3(-5, 5, 5); // Face right
            }
        }
    }

    void SetNextAbilityTime()
    {
        nextAbilityTime = Time.time + Random.Range(minCooldown, maxCooldown);
    }

    void TriggerAbility()
    {
        anim.SetTrigger("Ability");
        StartCoroutine(AbilitySequence());
    }

    IEnumerator AbilitySequence()
    {
        if (enemyScript != null)
        {
            enemyScript.SetAbilityActive(true);
        }

        // Create and show danger areas
        CreateDangerAreas();

        // Wait for the warning duration
        yield return new WaitForSeconds(warningDuration);

        // Spawn lightning and remove danger areas
        SpawnLightning();

        // Remove danger areas
        RemoveDangerAreas();

        // Wait for the ability duration
        yield return new WaitForSeconds(5f - warningDuration);

        if (enemyScript != null)
        {
            enemyScript.SetAbilityActive(false);
        }
    }

    void CreateDangerAreas()
{
    for (int i = 0; i < numberOfDangerAreas; i++)
    {
        float randomX = Random.Range(-8f, 8f);  
        Vector3 spawnPosition =  new Vector3(transform.position.x + randomX, 4.5f, 0);

        GameObject dangerArea = Instantiate(dangerAreaPrefab, spawnPosition, Quaternion.identity);
        dangerArea.transform.localScale = Vector3.one * dangerAreaRadius * 2;
        activeDangerAreas.Add(dangerArea);
    }
}

    void SpawnLightning()
    {
        foreach (GameObject dangerArea in activeDangerAreas)
        {
            GameObject lightning = Instantiate(lightningPrefab, dangerArea.transform.position, Quaternion.identity);
            activeLightning.Add(lightning);
            StartCoroutine(RemoveLightningAfterDelay(lightning));
        }
    }

    void RemoveDangerAreas()
    {
        foreach (GameObject dangerArea in activeDangerAreas)
        {
            Destroy(dangerArea);
        }
        activeDangerAreas.Clear();
    }

    IEnumerator RemoveLightningAfterDelay(GameObject lightning)
    {
        yield return new WaitForSeconds(lightningDuration);
        if (lightning != null)
        {
            activeLightning.Remove(lightning);
            Destroy(lightning);
        }
    }
}