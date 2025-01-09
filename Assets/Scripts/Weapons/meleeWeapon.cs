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
        mousePosition.z = 0; // Asegurar que est� en el mismo plano del objeto de origen

        // Calcular la direcci�n desde el origen hacia el rat�n
        Vector2 direction = (mousePosition - origin.position).normalized;

        Vector2 def=new Vector2(1,0);
        float angle = Vector2.Angle(def, direction);

        // Determinar la direcci�n del �ngulo (positivo o negativo)
        float cross = Vector3.Cross(def, direction).z;
        if (cross < 0)
        {
            angle = -angle;  // Si el producto cruzado es negativo, invertir el �ngulo
        }
        // Crear la rotaci�n final basada en el �ngulo
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // Instanciar el efecto de golpe con la rotaci�n y posici�n correctas
        if (hitEffect != null)
        {
            Vector3 spawn = origin.position;
            spawn.z=-1;
            GameObject stick = Instantiate(hitEffect, spawn, rotation);
            Destroy(stick, 0.5f);  // Destruir el efecto despu�s de un corto tiempo (opcional)
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin.position, range);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyBehavior enemy = enemyCollider.GetComponent<EnemyBehavior>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Aplicar da�o al enemigo
                    Debug.Log($"Golpeado: {enemy.name}");
                }
                else
                {
                    Debug.LogWarning($"El objeto {enemyCollider.name} no tiene el componente EnemyBehavior.");
                }
            }
            
        }
    }
}
