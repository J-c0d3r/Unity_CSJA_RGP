using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float totalHealth = 100f;
    public float currentHealth;
    public float speed;
    public float rotSpeed;
    private float rotation;
    public float gravity;

    public float enemyDmg;

    public bool isAlive;
    private bool isReady;

    public Image healthBar;
    private Vector3 moveDirection;

    private CharacterController controller;
    private Animator anim;

    List<Transform> EnemiesList = new List<Transform>();
    public float ColliderRadius;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        currentHealth = totalHealth;
        isAlive = true;
    }


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Move();
            GetMouseInput();
        }        
    }

    void Move()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (!anim.GetBool("attacking"))
                {
                    anim.SetBool("walking", true);
                    anim.SetInteger("transition", 1);
                    moveDirection = Vector3.forward * speed;
                    moveDirection = transform.TransformDirection(moveDirection);
                }
                else
                {
                    anim.SetBool("walking", false);
                    moveDirection = Vector3.zero;
                    //StartCoroutine(Attack(1)); 
                }
            }            

            if (Input.GetKeyUp(KeyCode.W) && !anim.GetBool("attacking"))
            //if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                moveDirection = Vector3.zero;
            }
        }

        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if (!anim.GetBool("walking"))
                {
                    StartCoroutine("Attack");
                }
            }
        }
    }

    IEnumerator Attack()
    {
        if (!isReady && !anim.GetBool("hiting"))
        {
            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);

            yield return new WaitForSeconds(0.5f);

            GetEnemiesRange();

            foreach (Transform enemies in EnemiesList)
            {
                Enemy enemy = enemies.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetHit(enemyDmg);
                }
            }

            yield return new WaitForSeconds(0.83f);

            anim.SetInteger("transition", 0);
            anim.SetBool("attacking", false);
            isReady = false;
        }
    }

    void GetEnemiesRange()
    {
        EnemiesList.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                EnemiesList.Add(c.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, ColliderRadius);
    }

    public void GetHit(float dmg)
    {
        currentHealth -= dmg;

        healthBar.fillAmount = currentHealth / totalHealth;

        if (currentHealth > 0)
        {
            StopCoroutine("Attack");
            anim.SetInteger("transition", 3);
            anim.SetBool("hiting", true);
            StartCoroutine(RecoveryFromHit());
        }
        else
        {
            anim.SetInteger("transition", 4);
            isAlive = false;
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1.33f);
        anim.SetInteger("transition", 0);
        anim.SetBool("hiting", false);
        isReady = false;
        anim.SetBool("attacking", false);
    }

    public void IncreaseStats(float health, float increaseSpeed)
    {
        currentHealth += health;
        speed += increaseSpeed;
    }

    public void DecreaseStats(float health, float increaseSpeed)
    {
        currentHealth -= health;
        speed -= increaseSpeed;
    }
}
