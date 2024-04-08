using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float Speed = 1.0f;
    public float RotationSpeed = 1.0f;
    public float JumpForce = 1.0f;

    private Rigidbody Physics;
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Physics = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0.0f, vertical) * Time.deltaTime * Speed);

        // Rotación
        float rotationY = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, rotationY * Time.deltaTime * RotationSpeed, 0));

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Physics.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            isGrounded = false; // El jugador no está en el suelo después de saltar
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el jugador ha colisionado con un objeto que representa el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // El jugador está en el suelo
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Verificar si el jugador ha dejado de colisionar con un objeto que representa el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // El jugador ya no está en el suelo
        }
    }
}
