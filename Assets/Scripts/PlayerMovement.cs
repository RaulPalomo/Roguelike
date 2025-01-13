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
    public float moveSpeed = 5f;             // Velocidad del jugador
    private Vector2 movementInput;          // Entrada del movimiento
    private Rigidbody2D rb;                 // Referencia al Rigidbody2D
    private Animator animator;              // Referencia al Animator
    private weaponController weaponController;
    bool isDead = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Player already exists, destroying new player object");
            Destroy(gameObject);  
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
        playerControls = new PlayerControls();
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponController = GetComponent<weaponController>();
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
        playerControls.Disable();
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
        playerControls.Player.shot.performed -= OnShot;
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
        Debug.Log("Auch");
        if (lives == 0)
        {
            isDead = true;
            Die();
        }
        Debug.Log("Dead: "+isDead);
    }
    public void Die()
    {
        // Aquí se puede agregar lógica adicional para la muerte del jugador
        isDead=true;
        Debug.Log("Player died!");
    }
}
