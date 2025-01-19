using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private EnemyBehavior enemyBehavior;
    private GameObject target;
    private Animator animator;

    void Awake()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        animator = GetComponent<Animator>();
        target= GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyBehavior.health > 0)
            {
                target = collision.gameObject;
                animator.SetBool("IsChasing", true);
                enemyBehavior.currentState = EnemyBehavior.EnemyState.Chase;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyBehavior.health > 0)
            {
                animator.SetBool("IsChasing", false);
                enemyBehavior.currentState = EnemyBehavior.EnemyState.Idle;
            }
        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

            if (enemyBehavior.health > 0)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    animator.SetBool("Attack", true);
                    enemyBehavior.currentState = EnemyBehavior.EnemyState.Attack;
                }
            }

        
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemyBehavior.health > 0)
            {
                animator.SetBool("Attack", false);
                enemyBehavior.currentState = EnemyBehavior.EnemyState.Chase;
            }
        }
        
        
    }

}
