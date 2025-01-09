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
        mousePosition.z = 0; // Asegurar que esté en el mismo plano del objeto de origen

        // Calcular la dirección desde el origen hacia el ratón
        Vector2 direction = (mousePosition - origin.position).normalized;

        Vector2 def=new Vector2(1,0);
        float angle = Vector2.Angle(def, direction);

        // Determinar la dirección del ángulo (positivo o negativo)
        float cross = Vector3.Cross(def, direction).z;
        if (cross < 0)
        {
            angle = -angle;  // Si el producto cruzado es negativo, invertir el ángulo
        }
        // Crear la rotación final basada en el ángulo
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // Instanciar el efecto de golpe con la rotación y posición correctas
        if (hitEffect != null)
        {
            Vector3 spawn = origin.position;
            spawn.z=-1;
            GameObject stick = Instantiate(hitEffect, spawn, rotation);
            Destroy(stick, 0.5f);  // Destruir el efecto después de un corto tiempo (opcional)
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin.position, range);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyBehavior enemy = enemyCollider.GetComponent<EnemyBehavior>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Aplicar daño al enemigo
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
