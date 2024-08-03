using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public enum Direction
    {
        Down,
        Left,
        Right,
        Up
    }
   
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public int hp;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public Animator anim;
    [SerializeField] public Slider hpSlider;
    [SerializeField] private GameObject[] weapons;
    private int weaponIndex = 0;
    [SerializeField] private Vector3 shootOffset = new Vector3(0.5f, 0, 0);
    [SerializeField] private float attackCooldown = 0.5f;
  
    private bool canAttack = true;

    private Vector2 moveDirection;
    private Direction lastDirection = Direction.Down;
    private bool isMoving = false;
    private Transform pickaxeTransform;
 
    void Update()
{
    if (!GameManager.isPaused)
    {
        ProcessInputs();
        Animate();
        CheckAttack();
    }
    CheckTreeInteraction();
}

void FixedUpdate()
{
    if (!GameManager.isPaused)
    {
        Move();
    }
}
 void CheckTreeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D obj in nearbyObjects)
            {
                if (obj.CompareTag("Tree"))
                {
                    GameManager.instance.ShowUpgradePanel();
                    GameManager.isPaused = true;
                    break;
                }
                else if (obj.CompareTag("MiningRock"))
                {
                    
                    StartCoroutine(ShowPickaxeTemporarily(obj.gameObject, obj.transform.position));
                   
                    break;
                }
            }
        }
    }
    private IEnumerator ShowPickaxeTemporarily(GameObject obj, Vector3 position)
{
    GameManager.isPaused = true;
    GameManager.instance.ShowPickaxe(position);
    yield return new WaitForSeconds(3f);
    GameManager.instance.HidePickaxe();
    GameManager.isPaused = false;
    Destroy(obj);
    GameManager.instance.increaseMineral();
    GameManager.instance.ShowMineralPanel();
}

   
    public int HP { get { return hp; } }

    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;
        hp = Mathf.Max(hp, 0);
        UpdateHealthSlider();
        UpgradeManager.Instance.UpdateHealthBarValue();
        Debug.Log("Health: " + hp);
        if (hp <= 0)
        {
            GameManager.instance.SetGameOver();
            Debug.Log("Player died!");
        }
    }
    public void UpdateHealthSlider()
{
    if (hpSlider != null)
    {
        hpSlider.maxValue = maxHealth;
        hpSlider.value = hp;
    }
}

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        isMoving = moveDirection != Vector2.zero;

        if (isMoving)
        {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                lastDirection = (moveDirection.x > 0) ? Direction.Right : Direction.Left;
            }
            else
            {
                lastDirection = (moveDirection.y > 0) ? Direction.Up : Direction.Down;
            }
        }
    }

    void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }


    void Animate()
    {
        if (isMoving)
        {
            anim.enabled = true;
            anim.SetFloat("AnimMoveX", moveDirection.x);
            anim.SetFloat("AnimMoveY", moveDirection.y);
        }
         else
    {
        anim.enabled = false;
    }
    }

    public void InitializeHPBar()
    {
        if (hpSlider != null)
        {
            
            hpSlider.value = hp;
            Debug.Log("HP Bar initialized for " + gameObject.name);
        }
        else
        {
            Debug.LogError("Cannot initialize HP Bar for " + gameObject.name + ": Slider is null");
        }
    }

    void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            Shoot();
            StartCoroutine(AttackCooldown());
        }
    }

    void Shoot()
    {
        Vector3 shootPosition = transform.position; // Initialize with player's position
        Quaternion shootRotation = Quaternion.identity; // Default rotation

        switch (lastDirection)
        {
            case Direction.Up:
                shootPosition += new Vector3(0, shootOffset.y, 0);
                shootRotation = Quaternion.Euler(0, 0, 45);
                break;
            case Direction.Down:
                shootPosition += new Vector3(0, -shootOffset.y, 0);
                shootRotation = Quaternion.Euler(0, 0, 45);
                break;
            case Direction.Left:
                shootPosition += new Vector3(-shootOffset.x, 0, 0);
                shootRotation = Quaternion.Euler(0, 0, -45);
                break;
            case Direction.Right:
                shootPosition += new Vector3(shootOffset.x, 0, 0);
                shootRotation = Quaternion.Euler(0, 0, -45);
                break;
        }

        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            GameObject weaponInstance = Instantiate(weapons[weaponIndex], shootPosition, shootRotation);
            Weapon weaponScript = weaponInstance.GetComponent<Weapon>();
        if (weaponScript != null)
        {
        weaponScript.fireDirection = lastDirection;
        }          
            Destroy(weaponInstance, 1f); // Destroy the weapon after 3 seconds
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
     private void OnTriggerEnter2D(Collider2D other) 
{
    if (other == null) return;

    if (other.CompareTag("Enemy")) 
    {
    } else if (other.CompareTag("EnemyWeapon"))
{
    EnemyWeapon enemyWeapon = other.GetComponent<EnemyWeapon>();
    if (enemyWeapon != null)
    {
        TakeDamage(enemyWeapon.damage);
    }
}
    else if (other.CompareTag("CopperCoin")) 
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.increaseCopperCoin();
        }
        else
        {
            Debug.LogWarning("GameManager instance is null. Cannot increase coin count.");
        } 
        Destroy(other.gameObject);
    } else if (other.CompareTag("SilverCoin")) 
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.increaseSilverCoin();
        }
        else
        {
            Debug.LogWarning("GameManager instance is null. Cannot increase coin count.");
        } 
        Destroy(other.gameObject);
    } else if (other.CompareTag("GoldCoin")) 
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.increaseGoldCoin();
        }
        else
        {
            Debug.LogWarning("GameManager instance is null. Cannot increase coin count.");
        } 
        Destroy(other.gameObject);
    } else if (other.CompareTag("EntranceOfMap2")) 
    {
        SceneManager.LoadScene("Map2");
    }
     else if (other.CompareTag("EntranceOfMap0")) 
    {
        SceneManager.LoadScene("Map0");
    }
     else if (other.CompareTag("EntranceOfMap1")) 
    {
        SceneManager.LoadScene("GameStartScene");
    }
    else if (other.CompareTag("EntranceOfSlimeCave"))
    {
        SceneManager.LoadScene("SlimeCave");
    } 
     else if (other.CompareTag("EntranceOfSlimeCave2F"))
    {
        SceneManager.LoadScene("SlimeCave2F");
    }
    else if (other.CompareTag("EntranceOfMap3"))
    {
        SceneManager.LoadScene("Map3");
    } 

}
}