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

    private void Awake()
    {
        playerControls = new PlayerControls();
        // Obtiene las referencias al Rigidbody2D y Animator
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Move.performed += OnMove;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Move.performed -= OnMove;
    }

    // Este método será llamado por Unity Events (vinculado a la acción Move)
    public void OnMove(InputAction.CallbackContext context)
    {
        // Lee el Vector2 desde el contexto
        movementInput = context.ReadValue<Vector2>();
        Debug.Log($"Input recibido: {movementInput}");
    }

    private void Update()
    {
        // Actualiza los parámetros del Animator con la entrada del jugador
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.y);

        // Determina si el jugador está en movimiento
        bool isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate()
    {
        // Aplica movimiento al Rigidbody2D
        rb.velocity = movementInput * moveSpeed;
    }
}
