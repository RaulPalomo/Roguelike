using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewFlamethrower", menuName = "Weapons/Flamethrower")]
public class Flamethrower : WeaponSO
{
    public GameObject flameEffect;

    public override void Use(Transform origin)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // Obtén la posición del mouse en el mundo
        mousePosition.z = 0f;
        Vector2 direction = (mousePosition - origin.position).normalized;
        Vector2 def=new Vector2(0,1);
        float angle = Vector2.Angle(def, direction);

        // Determinar la dirección del ángulo (positivo o negativo)
        float cross = Vector3.Cross(def, direction).z;
        if (cross < 0)
        {
            angle = -angle;  // Si el producto cruzado es negativo, invertir el ángulo
        }
        Quaternion rotation = Quaternion.Euler(0,0,angle);
        Vector3 spawn = origin.position+(Vector3)direction*0.7f;
        GameObject player= GameObject.FindGameObjectWithTag("Player");
        GameObject effect = Instantiate(flameEffect, spawn,rotation,player.transform);
        Destroy(effect, 2f);
        

        /*
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin.position, direction, 10f);
        
        foreach (RaycastHit2D hit in hits)
        {
            
            
            Debug.Log($"Quemado: {hit.collider.name}");
        }
        */

    }
}
