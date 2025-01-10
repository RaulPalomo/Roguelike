using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    public sniperWeapon sniperWeapon;  // Asigna el ScriptableObject del arma desde el Inspector
    public meleeWeapon meleeWeapon;    // Asigna el ScriptableObject del arma desde el Inspector
    public Flamethrower flamethrower;  // Asigna el ScriptableObject del arma desde el Inspector
    public Transform weaponOrigin; // Punto de origen para disparar las balas
    public int currentWeapon;
    private float lastShotTime; // Tiempo del último disparo
    private PlayerControls playerControls;

    public bool isSniperUnlocked = true;  // Controla si el arma está desbloqueada
    public bool isMeleeUnlocked = true;   // Controla si el arma está desbloqueada
    public bool isFlamethrowerUnlocked = true;  // Controla si el arma está desbloqueada

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.ChangeWeapon.performed += OnChangeWeapon;
    }

    private void OnDisable()
    {
        playerControls.Player.ChangeWeapon.performed -= OnChangeWeapon;
        playerControls.Player.Disable();
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        int weaponIndex = Mathf.FloorToInt(context.ReadValue<float>());
        Debug.Log("Cambiando a arma " + weaponIndex);
        if (weaponIndex == 0 && isMeleeUnlocked)
        {
            currentWeapon = 0;
            Debug.Log("Cambiado a arma cuerpo a cuerpo");
        }
        else if (weaponIndex == 1 && isSniperUnlocked)
        {
            currentWeapon = 1;
            Debug.Log("Cambiado a francotirador");
        }
        else if (weaponIndex == 2 && isFlamethrowerUnlocked)
        {
            currentWeapon = 2;
            Debug.Log("Cambiado a lanzallamas");
        }
        else
        {
            Debug.LogWarning("Arma no desbloqueada.");
        }
    }

    public void UseWeapon()
    {
        switch (currentWeapon)
        {
            case 0:
                UseMeleeWeapon();
                break;
            case 1:
                UseSniperWeapon();
                break;
            case 2:
                UseFlamethrower();
                break;
        }
    }
    public void UseSniperWeapon()
    {
        if (sniperWeapon != null)
        {
            if (Time.time - lastShotTime < sniperWeapon.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            sniperWeapon.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }

    public void UseMeleeWeapon()
    {
        if (meleeWeapon != null)
        {
            if (Time.time - lastShotTime < meleeWeapon.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            meleeWeapon.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }

    public void UseFlamethrower()
    {
        if (flamethrower != null)
        {
            if (Time.time - lastShotTime < flamethrower.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            flamethrower.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }
    /*void Start()
    {
        playerControls = new PlayerControls();
    }
    private void Update()
    {
        
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void UseWeapon()
    {
        switch (currentWeapon)
        {
            case 0:
                UseMeleeWeapon();
                break;
            case 1:
                UseSniperWeapon();
                break;
            case 2:
                UseFlamethrower();
                break;
        }

    }

    public void UseSniperWeapon()
    {
        if (sniperWeapon != null)
        {
            if (Time.time - lastShotTime < sniperWeapon.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            sniperWeapon.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }

    public void UseMeleeWeapon()
    {
        if (meleeWeapon != null)
        {
            if (Time.time - lastShotTime < meleeWeapon.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            meleeWeapon.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }

    public void UseFlamethrower()
    {
        if (flamethrower != null)
        {
            if (Time.time - lastShotTime < flamethrower.cooldown)
            {
                return;
            }
            lastShotTime = Time.time;
            flamethrower.Use(weaponOrigin);  // Llama al método de disparo definido en el ScriptableObject
        }
        else
        {
            Debug.LogWarning("No weapon equipped!");
        }
    }*/
}
