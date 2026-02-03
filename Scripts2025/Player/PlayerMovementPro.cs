using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementPro : MonoBehaviour
{
    // publicas
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    public float groundDrag = 5f;

    [Header("Jumping")]
    public float JumpForce = 20f;
    public float JumpCooldown = 0.25f;
    public float AirMultiplier = 0.4f;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Fisicas")]
    public float GravityMultiplier = 3f;
    public float Rozamiento = 4f;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public KeyCode SprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerheight = 2f;
    public LayerMask WhatIsGround;
    public bool Grounded;
    public Transform orientation;

    [Header("Caracteristicas")]
    public float alturaAgachado = 0.75f;
    public float duracionTransicion = 0.25f;

    [Header("Stamina Bar")]
    public Image StaminaBar;
    public float Stamina, MaxStamina = 100;
    public float AttackCost = 25;
    public float RunCost = 20;
    public float ChargeRate = 25;
    public bool isTired = false;
    public bool Aparcao = true;
    // privadas
    float horizontalInput;
    float VerticalInput;

    Vector3 moveDirection;
    // Mana
    private Coroutine recharge;

    // RigidBody
    Rigidbody rb;
    private Animator animator;

    public MovementState state;
    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        crouching,
        air
    }
    void Start()
    {
        startYScale = transform.localScale.y;
        // Obtenemos el componente rigidBody y le congelamos la rotaci�n :)
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // detectar si el jugador esta tocando el suelo: basicamente esta constantemente comprobando si el jugador a atravesado la malla de (WhatIsGround) si es asi, grounded = true
        Grounded = Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.2f, WhatIsGround);
        MyInput();
        SpeedControl();
        
        // manejar el drag: sin esto el movimiento del jugador parece que esta esquiando en hielo
        if (Grounded)
            rb.linearDamping = groundDrag;
        else rb.linearDamping = 0;
    }
    private void FixedUpdate() // Funcion como el update (loop sincronizado con el numero de FPS) pero estable
    {
        MovePlayer();
        StateHandler();
        VerSpeed();
        // Gravedad realista
        rb.AddForce(Physics.gravity * GravityMultiplier, ForceMode.Acceleration);
    }
    private void MyInput() // Funcion para recibir todos los inputs
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && Grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), JumpCooldown);
        }
        if (Input.GetKeyDown(CrouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(CrouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
        if (Input.GetKey(SprintKey) && rb.linearVelocity.magnitude != 0)
        {
            Sprinting();
        }

        animator.SetFloat("moveX", horizontalInput, 0.1f, Time.deltaTime);
        animator.SetFloat("moveZ", VerticalInput, 0.1f, Time.deltaTime);

    }
    private void StateHandler()
    {
        bool isMoving = horizontalInput == 0f && VerticalInput == 0f ? false : true;
        // Mode - crouching
        if (Input.GetKey(CrouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Mode - sprinting
        else if (Grounded && Input.GetKey(SprintKey) && !isTired)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Mode - walking/idle
        else if (Grounded)
        {
            moveSpeed = walkSpeed;
            if (isMoving)
            {
                // Walking
                state = MovementState.walking;
            } else
            {
                // idle
                state = MovementState.idle;
            }
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }
    private void MovePlayer() // Funcion destinada a realizar todo lo relacionado con el movimiento del jugador <3
    {
        moveDirection = orientation.forward * VerticalInput + orientation.right * horizontalInput;

        // En el suelo
        if (Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // En el aire
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * AirMultiplier, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limiar la velocidad del jugador ya que no es realmente preciso y se pasa de la velocidad establecida
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    private void VerSpeed()
    {
        var vel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var speed = vel.magnitude;

        Debug.Log(speed);
    }
    private void Jump()
    {
        // reseteamos la velocidad del eje y (arriba y abajo) justo antes de saltar para asegurarnos de que el salto siempre sea igual
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    
    private void Sprinting()
    {
        DrainStamina();
        if (recharge != null) StopCoroutine(recharge);
        recharge = StartCoroutine(RechargeStamina());

    }
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 40f;
            if (Stamina >= MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            if (Stamina >= MaxStamina * .2f) Aparcao = true;
            
            yield return new WaitForSeconds(.025f);
        }
    }
    private void DrainStamina()
    {
        Stamina -= RunCost * Time.deltaTime;
        if (Stamina <= 0)
        {
            Stamina = 0f;
            isTired = true;
        } else
        {
            isTired = false;
        }
        StaminaBar.fillAmount = Stamina / MaxStamina;
    }
    IEnumerator TransicionEscalado(Vector3 escalaInicial, Vector3 escalaFinal, float duracion)
    {
        float tiempoPasado = 0f;
        while (tiempoPasado < duracion)
        {
            tiempoPasado += Time.deltaTime;
            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempoPasado / duracion);
            yield return null;
        }
        transform.localScale = escalaFinal;
    }

}

