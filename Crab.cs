using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public Transform player;
        private Enemy enemyScript;

    // Start is called before the first frame update
      void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyScript = GetComponent<Enemy>();
    }


    // Update is called once per frame
    void Update()
    { 
        if (player != null)
        {
            // Flip the sprite based on player's position
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-5, 5, 5); // Face left
            }
            else
            {
                transform.localScale = new Vector3(5, 5, 5); // Face right
            }
        }
    }
}
