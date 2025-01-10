using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSniperWeapon", menuName = "Weapons/Sniper Weapon")]
public class sniperWeapon : WeaponSO
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
    
    public override void Use(Transform origin)
    {
       

        Debug.Log("Sniper weapon used");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // Obtén la posición del mouse en el mundo
        mousePosition.z = 0f;
        Vector2 direction = (mousePosition - origin.position).normalized;             // Calcula la dirección hacia el cursor
        Vector3 spawn = origin.position + (Vector3)direction *0.7f;
        spawn.z = -2f;
        GameObject bullet = Instantiate(bulletPrefab, spawn, Quaternion.identity);
        bullet.GetComponent<bulletBehaviour>().damage = damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;  // Aplica la velocidad en la dirección calculada
        }
    }
}
