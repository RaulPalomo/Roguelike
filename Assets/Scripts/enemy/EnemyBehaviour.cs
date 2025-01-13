using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState { Idle, Chase, Attack, Dead }
    public EnemyState currentState = EnemyState.Idle;

    public float health = 100f;
    public float speed = 3f;
    private Transform player;
    private Animator animator;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentState != EnemyState.Dead)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    IdleState();
                    break;
                case EnemyState.Chase:
                    ChaseState();
                    break;
                case EnemyState.Attack:
                    AttackState();
                    break;
                case EnemyState.Dead:
                    DeadState();
                    break;
            }
        }
        else
        {
            DeadState();
        }
    }

    public void IdleState()
    {
        
    }

    public void ChaseState()
    {
       
        Vector3 direction = player.position - transform.position;
        if(direction.x < 0)
        {
            
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;   
        }
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    public void AttackState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(WaitAnimationAttack(player));
    }
    private IEnumerator WaitAnimationAttack(GameObject player)
    {
        yield return new WaitForSeconds(0.8f);
        if (currentState == EnemyState.Attack)
        {
            player.GetComponent<PlayerMovement>().TakeDamage();
            Debug.Log("Player lives: " + player.GetComponent<PlayerMovement>().lives);
            health-=100;
            currentState = EnemyState.Dead;
        }
        
    }

    public void DeadState()
    {
        animator.SetBool("IsDead", true);
        Destroy(gameObject, 2f);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            
            currentState = EnemyState.Dead;
        }
        Debug.Log("Enemy health: " + health);
    }
    public void OnDestroy()
    {
        EventController.TriggerEnemyDefeat();
    }
}
