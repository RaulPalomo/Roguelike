using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    public float damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Player"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyBehavior>().TakeDamage(damage);
            }
            Debug.Log("Hit: " + collision.name);
            Destroy(gameObject);
        }
        
    }
}
