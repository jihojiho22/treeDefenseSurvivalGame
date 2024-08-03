using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public Rigidbody2D enemyRb;
    
    [SerializeField] public Animator anim;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;    
    [SerializeField] private int hp;
    [SerializeField] private int maxHealth;
    [SerializeField] private float attackRange;
    [SerializeField] public Slider hpSlider;
    [SerializeField] private GameObject coin;
    [SerializeField] private AudioSource attackAudioSource;

    private Transform treeTransform;
    private Transform playerTransform;
    private Vector2 moveDirection;
    private float stepInterval = 0.5f; // Time between direction changes
    private float stepTimer;
    private MainTree treeScript;
    private Player playerScript;
    private bool isAttackOnCooldown = false;
    private bool isAbilityActive = false;

    void Awake()
    {
        StartCoroutine(InitializeReferences());
    }

    IEnumerator InitializeReferences()
    {
        while (treeTransform == null || playerTransform == null)
        {
            GameObject tree = GameObject.FindGameObjectWithTag("Tree");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
           
            if (tree != null)
            {
                treeTransform = tree.transform;
                treeScript = tree.GetComponent<MainTree>();
            }

            if (player != null)
            {
                playerTransform = player.transform;
                playerScript = player.GetComponent<Player>();
            }

            if (treeTransform == null || playerTransform == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        Debug.Log("Tree script found: " + (treeScript != null));
        Debug.Log("Player script found: " + (playerScript != null));
    }

    void Start()
    {
          if (hpSlider != null)
        {
        hpSlider.maxValue = hp;
        hpSlider.value = hp;
        }
        GameObject tree = GameObject.FindGameObjectWithTag("Tree");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (tree != null)
        {
            treeTransform = tree.transform;
            treeScript = tree.GetComponent<MainTree>();
            Debug.Log("Tree script found: " + (treeScript != null));
        }
        if (player != null)
        {
            playerTransform = player.transform;
            playerScript = player.GetComponent<Player>();
            Debug.Log("Player script found: " + (playerScript != null));
        }
        
     enemyRb = GetComponent<Rigidbody2D>();
        stepTimer = stepInterval;
    }

    void Update()
{
    if (isAbilityActive)
        {
            // Don't move during ability
            if (enemyRb != null)
            {
                enemyRb.velocity = Vector2.zero;
            }
            return;
        }
         if (IsCloseEnoughToAttack() && !isAttackOnCooldown && GameManager.isPaused == false)
    {
        StartCoroutine(AttackCooldown());
    }
    else
    {
        // Move towards player if tree doesn't exist
        if (treeTransform == null && playerTransform != null)
        {
            moveDirection = (playerTransform.position - transform.position).normalized;
        }
        else if (treeTransform != null)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                ChooseNewDirection();
                stepTimer = stepInterval;
            }
        }

        if (enemyRb != null && GameManager.isPaused == false)
        {
            enemyRb.velocity = (moveDirection) * moveSpeed;
            Animate();
        }
    }
   
}

public void SetAbilityActive(bool active)
    {
        isAbilityActive = active;
        if (active)
        {
            // Stop movement immediately when ability starts
            if (enemyRb != null)
            {
                enemyRb.velocity = Vector2.zero;
            }
        }
    }

  
    public void InitializeHPBar()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = hp;
            Debug.Log("HP Bar initialized for " + gameObject.name);
        }
        else
        {
            Debug.LogError("Cannot initialize HP Bar for " + gameObject.name + ": Slider is null");
        }
    }

   private void Attack()
{
    if (anim != null) {
        anim.SetTrigger("Attack");
        anim.SetTrigger("Hit");
    }
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
    if (treeTransform != null && GameManager.isPaused == false)
    {
       
       float distanceToTree = Vector2.Distance(transform.position, treeTransform.position);

        if (distanceToPlayer <= attackRange && playerScript != null)
        {
            Debug.Log("Attacking player");
            playerScript.TakeDamage(damage);
            attackAudioSource.Play();
        }
        else if (distanceToTree <= attackRange && treeScript != null)
        {
            Debug.Log("Attacking tree");
            treeScript.TakeDamage(damage);
            attackAudioSource.Play();
        }
    } else if (treeTransform == null || playerTransform != null) {
        if (distanceToPlayer <= attackRange && playerScript != null)
        {
            Debug.Log("Attacking player");
            playerScript.TakeDamage(damage);
            attackAudioSource.Play();
        }
    }
}


 private IEnumerator AttackCooldown()
    {
        isAttackOnCooldown = true;
        Attack();
        yield return new WaitForSeconds(attackCooldown);
       
        isAttackOnCooldown = false;
    }

    private bool IsCloseEnoughToAttack()
{
    
    if (playerTransform != null && treeTransform == null)
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= attackRange;
    } else if (playerTransform == null || treeTransform == null)
    {
        return false;
    } else {
         return Vector2.Distance(transform.position, playerTransform.position) <= attackRange || Vector2.Distance(transform.position, treeTransform.position) <= attackRange;
    }
}

    void ChooseNewDirection()
{
    if (treeTransform == null || playerTransform == null)
    {
        Debug.LogWarning("Tree or Player transform is null. Skipping direction change.");
        return;
    }

    Vector2 directionToTree = (treeTransform.position - transform.position).normalized;
    Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

    if ( playerTransform != null && treeTransform == null)
    {
        // Set direction towards player
    moveDirection = directionToPlayer;
    }
    else if ( playerTransform != null && treeTransform != null) 
    {
        if ((Vector2.Distance(transform.position, playerTransform.position) >= 3.5f)) {
            moveDirection = directionToTree;
        } else {
            moveDirection = directionToPlayer;
        }
    }



    
    if (Random.value > 0.5f)
    {
        // Move horizontally
        moveDirection = new Vector2(Mathf.Sign(moveDirection.x), 0);
    }
    else
    {
        // Move vertically
        moveDirection = new Vector2(0, Mathf.Sign(moveDirection.y));
    }
}

    void Animate()
{
    if (anim == null)
    {
        Debug.LogError("Animator is null!");
        return;
    } 
    anim.SetFloat("AnimMoveX", moveDirection.x);
    anim.SetFloat("AnimMoveY", moveDirection.y);
    
   
}

public void UpdateHealthSlider()
{
    if (hpSlider != null)
    {
        hpSlider.value = hp;
    }
}

    private void OnTriggerEnter2D(Collider2D other) {
        //taking damage from the weapon
        if(other.gameObject.tag == "Weapon") {
            Weapon weapon = other.GetComponent<Weapon>();
            hp -= weapon.damage;
            UpdateHealthSlider();
            if(hp <= 0){
                Destroy(gameObject);
                Instantiate(coin, transform.position, Quaternion.identity);
            }
            Destroy(other.gameObject);
        }
    }

}