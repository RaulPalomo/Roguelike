using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }
    PlayerControls playerControls;
    public int lives = 3;
    public int coins = 0;
    public int loop = 1;
    public float moveSpeed = 5f;             
    private Vector2 movementInput;          
    private Rigidbody2D rb;                 
    private Animator animator;              
    private weaponController weaponController;
    bool isDead = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            
            Destroy(gameObject);
            
            return;

        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
        playerControls = new PlayerControls();
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponController = GetComponent<weaponController>();
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 0, 0);

    }
    private void Start()
    {
        weaponController.isMeleeUnlocked = false;
        weaponController.isSniperUnlocked = false;
        weaponController.isFlamethrowerUnlocked = false;
        weaponController.currentWeapon=Random.Range(0,2);
        
        if (weaponController.currentWeapon == 0)
        {
            weaponController.isMeleeUnlocked = true;
        }
        else if (weaponController.currentWeapon == 1)
        {
            weaponController.isSniperUnlocked = true;
        }
        else if (weaponController.currentWeapon == 2)
        {
            weaponController.isFlamethrowerUnlocked = true;
        }
    }
    private void OnEnable()
    {
        
        playerControls.Enable();
        
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
        playerControls.Player.shot.performed += OnShot;

    }
    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
            playerControls.Player.Move.performed -= OnMove;
            playerControls.Player.Move.canceled -= OnMove;
            playerControls.Player.shot.performed -= OnShot;
        }
    }

    // Este método será llamado por Unity Events (vinculado a la acción Move)
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return;
        // Lee el Vector2 desde el contexto
        movementInput = context.ReadValue<Vector2>();
        
    }
    public void OnShot(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed)
        {
            weaponController.UseWeapon();  // Llama a la función de disparo en el controlador de armas
        }
    }

    private void Update()
    {
        if (isDead) return;
        // Actualiza los parámetros del Animator con la entrada del jugador
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.y);

        // Determina si el jugador está en movimiento
        bool isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        // Aplica movimiento al Rigidbody2D
        rb.velocity = movementInput * moveSpeed;
    }

    public void TakeDamage()
    {
        lives--;
        
        if (lives == 0)
        {
            isDead = true;
            Die();
        }
        
    }
    public void Die()
    {
        
        isDead=true;
        Debug.Log("Player died!");
    }
}
