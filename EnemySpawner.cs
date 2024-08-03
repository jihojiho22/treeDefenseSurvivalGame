using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField] private GameObject hpBarPrefab;

    private float[] arrPosX = {-10f, 10f};
    private float[] arrposY = {1.5f, 0.5f, -0.5f, -1.5f, -2.5f};
    
    void Start()
    {
        SpawnEnemies();
        InvokeRepeating("SpawnEnemies", 10f, 10f);
    }
    

   
void SpawnEnemies()
{
    int indexOfY = Random.Range(0,5);
    int enemyIndex = 0;
    foreach (float posX in arrPosX) {
        SpawnEnemy(posX, arrposY[indexOfY], enemyIndex);
    }
}
   void SpawnEnemy(float posX, float posY, int enemyIndex)
{
    Vector3 spawnPos = new Vector3(posX, posY, 0f);
    GameObject enemyInstance = Instantiate(enemies[enemyIndex], spawnPos, Quaternion.identity);
    Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
    
    if (enemyScript != null)
    {
        enemyScript.InitializeHPBar();
        Debug.Log("Enemy spawned: " + enemyInstance.name);
    }
    else
    {
        Debug.LogError("Enemy script not found on instantiated enemy: " + enemyInstance.name);
    }
    
}
}
