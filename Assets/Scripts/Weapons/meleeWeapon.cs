using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "Weapons/Melee Weapon")]
public class meleeWeapon : WeaponSO
{
    public float range;

    public override void Use(Transform origin)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin.position, range);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Aplicar daño al enemigo
            Debug.Log($"Golpeado: {enemy.name}");
        }
    }
}
