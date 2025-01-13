using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretBulletBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().TakeDamage();
            Destroy(gameObject);
        }
        if (!collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        
    }
}
