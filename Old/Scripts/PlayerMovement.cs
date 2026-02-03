using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float forwardSpeed = 6f; // Velocidad al moverse hacia adelante
    public float backwardSpeed = 3f; // Velocidad al moverse hacia atrás
    public float sprintForwardSpeed = 12f; // Velocidad al sprintear hacia adelante
    public float sprintBackwardSpeed = 6f; // Velocidad al sprintear hacia atrás
    public float crouchSpeed = 3f; // Velocidad al agacharse
    public float crouchSprintSpeed = 6f; // Velocidad al estar agachado y presionar Shift
    public float gravity = -20f;
    public float jumpHeight = 1f;

    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;
    private float initialControllerHeight; // Altura inicial del CharacterController

    void Start()
    {
        initialControllerHeight = controller.height; // Guardar la altura inicial
    }

    void Update()
    {
        // Verificar si el jugador está en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movimiento horizontal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Ajustar la velocidad de movimiento según el estado del jugador (de pie, agachado, corriendo)
        float currentSpeed;
        if (z < 0) // Si el jugador va hacia atrás
        {
            currentSpeed = isCrouching ? backwardSpeed : backwardSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = isCrouching ? crouchSprintSpeed : sprintBackwardSpeed;
            }
        }
        else // Si el jugador va hacia adelante
        {
            currentSpeed = isCrouching ? crouchSpeed : forwardSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = isCrouching ? crouchSprintSpeed : sprintForwardSpeed;
            }
        }

        // Aplicar movimiento
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Agacharse o levantarse al presionar la tecla de control
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                controller.height = 1f; // Agacharse
                transform.position -= new Vector3(0f, initialControllerHeight - 1f, 0f); // Mover al jugador hacia abajo
            }
            else
            {
                controller.height = initialControllerHeight; // Levantarse
                transform.position += new Vector3(0f, initialControllerHeight - 1f, 0f); // Mover al jugador hacia arriba
            }
        }

        // Saltar si se presiona la barra espaciadora y el jugador está en el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (isCrouching)
            {
                isCrouching = false;
                controller.height = initialControllerHeight; // Levantarse al saltar estando agachado
                transform.position += new Vector3(0f, initialControllerHeight - 1f, 0f); // Mover al jugador hacia arriba
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}








