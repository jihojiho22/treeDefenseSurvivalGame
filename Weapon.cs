using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player.Direction fireDirection;
    [SerializeField] public float projectileSpeed = 10f;
    [SerializeField] public int damage;
    // Start is called before the first frame update
    void Start()
    {
        FireProjectile();
    }

    // Update is called once per frame
    void Update()
    {
    }

          void FireProjectile()
    {
        Vector2 direction;
        switch (fireDirection)
        {
            case Player.Direction.Up:
                direction = Vector2.up;
                break;
            case Player.Direction.Down:
                direction = Vector2.down;
                break;
            case Player.Direction.Left:
                direction = Vector2.left;
                break;
            case Player.Direction.Right:
                direction = Vector2.right;
                break;
            default:
                direction = Vector2.zero;
                break;
        }

        GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}
