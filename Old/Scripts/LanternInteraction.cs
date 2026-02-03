using UnityEngine;

public class LanternInteraction : MonoBehaviour
{
    public KeyCode pickupKey = KeyCode.E; // Tecla para recoger la linterna
    public KeyCode toggleLightKey = KeyCode.F; // Tecla para encender/apagar la luz
    public KeyCode dropKey = KeyCode.G; // Tecla para soltar la linterna

    public float bobSpeed = 1f; // Velocidad de oscilación
    public float bobAmount = 0.1f; // Magnitud de oscilación

    private bool isLanternInInventory = false;
    private bool isLightOn = false;
    private Rigidbody rb;
    private Light flashlight;
    private Transform playerHands; // Punto de referencia para la posición de la linterna
    private Vector3 originalPosition; // Almacena la posición original de la linterna antes de recogerla
    private Quaternion originalRotation; // Almacena la rotación original de la linterna antes de recogerla

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        flashlight = GetComponentInChildren<Light>();
        if (flashlight != null)
            flashlight.enabled = false; // Apagar la luz al principio
        playerHands = GameObject.Find("PlayerHands").transform; // Asigna el punto de referencia de las manos del jugador
    }

    void Update()
    {
        // Recoger la linterna con la tecla E si no está en el inventario
        if (Input.GetKeyDown(pickupKey) && !isLanternInInventory)
        {
            isLanternInInventory = true;
            rb.isKinematic = true; // Hacer la linterna cinemática para que no caiga
            originalPosition = transform.localPosition; // Almacenar la posición original de la linterna
            originalRotation = transform.localRotation; // Almacenar la rotación original de la linterna
            transform.SetParent(playerHands); // Asignar la linterna como hijo del objeto de las manos del jugador
            transform.localPosition = Vector3.zero; // Posicionar la linterna en las manos del jugador
            transform.localRotation = Quaternion.identity; // Rotar la linterna según las manos del jugador
        }

        // Encender o apagar la luz con la tecla F si la linterna está en el inventario
        if (Input.GetKeyDown(toggleLightKey) && isLanternInInventory)
        {
            isLightOn = !isLightOn;
            if (flashlight != null)
                flashlight.enabled = isLightOn;
        }

        // Soltar la linterna con la tecla G si está en el inventario
        if (Input.GetKeyDown(dropKey) && isLanternInInventory)
        {
            isLanternInInventory = false;
            rb.isKinematic = false; // Hacer la linterna no cinemática para que caiga
            transform.SetParent(null); // Desasignar la linterna como hijo de las manos del jugador
            transform.localPosition = originalPosition; // Restablecer la posición original de la linterna
            transform.localRotation = originalRotation; // Restablecer la rotación original de la linterna
        }

        // Oscilar la linterna verticalmente
        if (isLanternInInventory)
        {
            float newY = originalPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
    }
}






