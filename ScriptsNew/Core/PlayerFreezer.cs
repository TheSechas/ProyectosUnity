using UnityEngine;

public class PlayerFreezer : MonoBehaviour
{
    [Header("Scripts a desactivar en el Player")]
    [SerializeField] private Behaviour[] scriptsToDisable; // Scripts del Player que quieres desactivar

    private Behaviour cameraLookScript; // Se buscará automáticamente en los hijos
    private Rigidbody body;

    private bool isFrozen = false;

    void Awake()
    {
        // Busca automáticamente el script de la cámara en los hijos.
        cameraLookScript = GetComponentInChildren<MouseLook>();
        if (cameraLookScript == null)
        {
            Debug.LogWarning("PlayerFreezer: No se encontró MouseLook en los hijos del Player.");
        }
        body = GetComponent<Rigidbody>();
    }

    public void Freeze()
    {
        // Congelamos el rigidbody para que no pueda rotar ni moverse.
        body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        if (isFrozen) return;

        // Desactiva los scripts del Player.
        foreach (var s in scriptsToDisable)
        {
            if (s != null) s.enabled = false;
        }
        
        
        // Desactiva el script de la cámara.
        if (cameraLookScript != null) cameraLookScript.enabled = false;

        // Muestra el cursor.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        isFrozen = true;
    }

    public void Unfreeze()
    {
        body.constraints = RigidbodyConstraints.FreezeRotation;
        if (!isFrozen) return;

        // Activa los scripts del Player.
        foreach (var s in scriptsToDisable)
        {
            if (s != null) s.enabled = true;
        }

        // Activa el script de la cámara.
        if (cameraLookScript != null) cameraLookScript.enabled = true;
        // Bloquea el cursor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isFrozen = false;
    }
}
