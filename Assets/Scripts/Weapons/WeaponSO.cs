using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSO : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float cooldown;
    public Sprite icon;

    // Método abstracto para usar el arma
    public abstract void Use(Transform origin);
}
