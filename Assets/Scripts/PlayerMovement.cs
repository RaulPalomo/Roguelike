using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls playerControls;

    public float moveSpeed = 5f;             // Velocidad del jugador
    private Vector2 movementInput;          // Entrada del movimiento
    private Rigidbody2D rb;                 // Referencia al Rigidbody2D
    private Animator animator;              // Referencia al Animator
    private weaponController weaponController;
    private void Awake()
    {
        playerControls = new PlayerControls();
        // Obtiene las referencias al Rigidbody2D y Animator
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

    // Este m�todo ser� llamado por Unity Events (vinculado a la acci�n Move)
    public void OnMove(InputAction.CallbackContext context)
    {
        // Lee el Vector2 desde el contexto
        movementInput = context.ReadValue<Vector2>();
        
    }
    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weaponController.UseWeapon();  // Llama a la funci�n de disparo en el controlador de armas
        }
    }

    private void Update()
    {
        // Actualiza los par�metros del Animator con la entrada del jugador
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.y);

        // Determina si el jugador est� en movimiento
        bool isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate()
    {
        // Aplica movimiento al Rigidbody2D
        rb.velocity = movementInput * moveSpeed;
    }
}
