using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour
{
    public sniperWeapon equippedWeapon;  // Asigna el ScriptableObject del arma desde el Inspector
    public Transform weaponOrigin;       // Punto de origen para disparar las balas
    private float lastShotTime;           // Tiempo del último disparo
    private void Update()
    {
        
    }

    public void UseWeapon()
    {
        if (equippedWeapon != null)
        {
            if (Time.time - lastShotTime < equippedWeapon.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            equippedWeapon.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }
}
