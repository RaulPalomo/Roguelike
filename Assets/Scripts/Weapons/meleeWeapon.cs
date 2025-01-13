using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Weapons/Melee Weapon")]
public class meleeWeapon : WeaponSO
{
    public float range;
    public GameObject hitEffect;
    public override void Use(Transform origin)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
         

        Vector2 direction = (mousePosition - origin.position).normalized;

        Vector2 def=new Vector2(1,0);
        float angle = Vector2.Angle(def, direction);


        float cross = Vector3.Cross(def, direction).z;
        if (cross < 0)
        {
            angle = -angle;
        }

        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);


        if (hitEffect != null)
        {
            Vector3 spawn = origin.position; 
            spawn.z=-1;
            GameObject stick = Instantiate(hitEffect, spawn, rotation);
            Destroy(stick, 0.5f);  
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin.position, range);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyBehavior enemy = enemyCollider.GetComponent<EnemyBehavior>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); 
                    Debug.Log($"Golpeado: {enemy.name}");
                }
                else
                {
                    TurretBehaviour turret = enemyCollider.GetComponent<TurretBehaviour>();
                    turret.TakeDamage(damage); 
                    Debug.Log($"Golpeado: {turret.name}");
                }
            }
            
        }
    }
}
