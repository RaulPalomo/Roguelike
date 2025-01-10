using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewFlamethrower", menuName = "Weapons/Flamethrower")]
public class Flamethrower : WeaponSO
{
    public ParticleSystem flameEffect;

    public override void Use(Transform origin)
    {
        ParticleSystem effect = Instantiate(flameEffect, origin.position, origin.rotation);
        effect.Play();

        // Detectar enemigos en un cono o área frente al lanzallamas
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin.position, origin.up, 10f);
        
        foreach (RaycastHit2D hit in hits)
        {
            
            Debug.Log($"Quemado: {hit.collider.name}");
        }
    }
}
