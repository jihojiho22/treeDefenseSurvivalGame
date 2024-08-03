using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CaveSlime : MonoBehaviour
{
    public GameObject slimeProjectile;
    public float shootingInterval = 2f;
    public float projectileSpeed = 5f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ShootAtPlayer", 0f, shootingInterval);
    }

    void ShootAtPlayer()
{
    if (playerTransform != null)
    {
       
        ShootProjectile(playerTransform.position);

        for (int i = 0; i < 2; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 3f; 
            Vector2 randomPosition = (Vector2)playerTransform.position + randomOffset;
            ShootProjectile(randomPosition);
        }
    }
}

void ShootProjectile(Vector2 targetPosition)
{
    Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
    GameObject projectile = Instantiate(slimeProjectile, transform.position, Quaternion.identity);
    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = direction * projectileSpeed;
    }
    Destroy(projectile, 3f); 
}
}

