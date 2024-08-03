using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningResource : MonoBehaviour
{
    [SerializeField] public Animator anim;
    private bool isMining = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void Animate()
    {
        if (isMining)
        {
            anim.enabled = true;
            // anim.SetFloat("AnimMoveX", moveDirection.x);
            // anim.SetFloat("AnimMoveY", moveDirection.y);
        }
         else
    {
        anim.enabled = false;
    }
    }
}
