using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float totalHealth = 100f;
    public float currentHealth;
    public float attackDmg;
    public float movSpeed;

    public float colliderRadius;

    private bool playerIsAlive;
    private bool isReady;

    private Animator anim;

    public float lookRadius = 10f;
    public Transform target;
    private NavMeshAgent agent;

    [Header("Life Bar")]
    public Image healthBar;
    public GameObject canvasBar;

    [Header("Path")]
    public List<Transform> pathPoints = new List<Transform>();
    public int currentPathIndex = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = totalHealth;
        playerIsAlive = true;
    }

    void MoveToNextPoint()
    {
        if (pathPoints.Count > 0)
        {
            float distance = Vector3.Distance(pathPoints[currentPathIndex].position, transform.position);
            agent.destination = pathPoints[currentPathIndex].position;

            if (distance <= 4f)
            {
                //currentPathIndex++;
                currentPathIndex = Random.Range(0, pathPoints.Count);
                currentPathIndex %= pathPoints.Count;
            }

            anim.SetInteger("transition", 2);
            anim.SetBool("walking", true);
        }
    }

    private void Update()
    {
        if (currentHealth > 0)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.isStopped = false;
                if (!anim.GetBool("attacking"))
                {
                    agent.SetDestination(target.position);
                    anim.SetInteger("transition", 2);
                    anim.SetBool("walking", true);
                }

                if (distance <= agent.stoppingDistance)
                {
                    StartCoroutine("Attack");
                    LookTarget();
                }
                else
                {
                    anim.SetBool("attacking", false);
                }

            }
            else
            {                
                anim.SetInteger("transition", 0);
                anim.SetBool("walking", false);
                anim.SetBool("attacking", false);
                //agent.isStopped = true;
                MoveToNextPoint();
            }
        }
    }

    IEnumerator Attack()
    {
        if (!isReady && playerIsAlive && !anim.GetBool("hiting"))
        {
            isReady = true;
            anim.SetBool("attacking", true);
            anim.SetBool("walking", false);
            anim.SetInteger("transition", 1);
            yield return new WaitForSeconds(1f);
            GetEnemy();
            yield return new WaitForSeconds(1.7f);
            isReady = false;
        }

        if (!playerIsAlive)
        {
            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            anim.SetBool("attacking", false);
            agent.isStopped = true;
        }
    }

    void GetEnemy()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
                c.gameObject.GetComponent<Player>().GetHit(25f);
                playerIsAlive = c.gameObject.GetComponent<Player>().isAlive;
            }
        }
    }

    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
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
            canvasBar.gameObject.SetActive(false);
            anim.SetInteger("transition", 4);
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(0.83f);
        anim.SetInteger("transition", 0);
        anim.SetBool("hiting", false);
        isReady = false;
    }


}
