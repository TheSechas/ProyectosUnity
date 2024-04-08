using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerBody;
    public float bobSpeed = 1f; // Velocidad de cabeceo
    public float bobAmount = 0.05f; // Cantidad de cabeceo
    public float sprintBobMultiplier = 2f; // Multiplicador de cabeceo al sprintear
    public float idleBobAmount = 0.01f; // Cantidad de movimiento de la cámara cuando el jugador está quieto
    public float moveBobAmount = 0.02f; // Cantidad de movimiento de la cámara al moverse
    public float sprintBobAmount = 0.03f; // Cantidad de movimiento de la cámara al sprintar

    private Vector3 initialPosition;
    private bool isGrounded;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Obtener la velocidad del jugador
        Vector3 playerVelocity = playerBody.GetComponent<CharacterController>().velocity;

        // Obtener la magnitud de la velocidad en el plano XZ (sin tener en cuenta el movimiento vertical)
        float playerSpeed = new Vector3(playerVelocity.x, 0f, playerVelocity.z).magnitude;

        // Comprobar si el jugador está en el suelo
        isGrounded = playerBody.GetComponent<CharacterController>().isGrounded;

        // Calcular la cantidad de cabeceo basada en la velocidad del jugador y si está sprintando
        float bobAmountMultiplier = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            bobAmountMultiplier = sprintBobMultiplier;
        }

        // Calcular la cantidad de movimiento de la cámara basada en si el jugador está quieto, en movimiento o sprintando
        float currentBobAmount = idleBobAmount;
        if (playerSpeed > 0.1f)
        {
            currentBobAmount = moveBobAmount;
        }
        currentBobAmount *= bobAmountMultiplier;

        // No aplicar cabeceo si el jugador está saltando
        if (isGrounded)
        {
            // Calcular el cabeceo
            float bobX = Mathf.Sin(Time.time * bobSpeed) * currentBobAmount;
            float bobY = Mathf.Cos(Time.time * bobSpeed * 2f) * currentBobAmount;

            // Aplicar el cabeceo a la posición de la cámara
            transform.localPosition = initialPosition + new Vector3(bobX, bobY, 0f);
        }
    }
}



