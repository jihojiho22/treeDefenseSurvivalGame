using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
   [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    void Jump() {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        float randomJumpForce = Random.Range(3f, 6f);
        Vector2 jumpVelocity = Vector2.up * randomJumpForce;
        jumpVelocity.x = Random.Range(-1f,1f);
        rigidBody.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

}
