using System.Collections;
using System.Collections.Generic;
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
    }

    private void Update()
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

    public void IdleState()
    {
        Debug.Log("Idle");
    }

    public void ChaseState()
    {
        Debug.Log("Chase");
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

        Debug.Log("Attack");
    }

    public void DeadState()
    {
        Debug.Log("Dead");
    }
}
