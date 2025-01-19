using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stick : MonoBehaviour
{
    public float damage = 10;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = collision.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                
            }
            else
            {
                TurretBehaviour turret = collision.GetComponent<TurretBehaviour>();
                turret.TakeDamage(damage);
                
            }
        }
    }
}
