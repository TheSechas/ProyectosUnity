using UnityEngine;
using TMPro;
using System.Collections;

public class InspectionSystem : MonoBehaviour
{
    public static InspectionSystem Instance;

    [Header("UI")]
    [SerializeField] private GameObject inspectionPanel;
    [SerializeField] private TextMeshProUGUI controlsText;
    [SerializeField] private TextMeshProUGUI translateText;

    [Header("Player")]
    [SerializeField] private PlayerFreezer playerFreezer;

    private Transform targetObject;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Transform originalParent;
    private string noteText;

    private bool isPickable = false;
    private Transform heldObject = null;
    public Transform HeldObject => heldObject;
    public bool IsInspecting { get; private set; }

    private PickableObject pickObj;
    private Vector3 previousMousePosition;
    void Awake() => Instance = this;

    // Inicia la inspección de un objeto (nota o ítem recogible).
    public void StartInspection(Transform obj, string text, bool isPickable = false)
    {
        if (IsInspecting) return;
        if (obj == null) return;

        targetObject = obj;
        this.isPickable = isPickable;
        noteText = text;
        originalPos = obj.position;
        originalRot = obj.rotation;
        originalParent = obj.parent;

        if (Camera.main == null) return;

        obj.SetParent(Camera.main.transform);
        obj.localPosition = new Vector3(0, 0, 0.25f);
        obj.localRotation = Quaternion.identity; // siempre recto

        if (!isPickable)
        {
            if (controlsText != null)
                controlsText.text = "Pulsa E para traducir\nClick derecho para salir";
        }
        else
        {
            if (controlsText != null)
                controlsText.text = $"Pulsa E para coger {obj.name}\nClick derecho para salir";

            // Desactivar físicas mientras inspeccionamos.
            Collider[] colliders = obj.GetComponents<Collider>();
            foreach (Collider c in colliders)
            {
                if (!c.isTrigger) c.enabled = false; // Solo desactivar collider físico.
            }

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }

        inspectionPanel?.SetActive(true);
        playerFreezer?.Freeze();
        UIManager.Instance?.HideAllCrosshairs();
        Cursor.visible = true;

        IsInspecting = true;
    }

    void Update()
    {
        if (!IsInspecting && heldObject == null) return;

        // --- Modo inspección ---
        if (IsInspecting && targetObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
                float rotationX = deltaMousePosition.y * 75f * Time.deltaTime;
                float rotationY = -deltaMousePosition.x * 75f * Time.deltaTime;

                Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
                targetObject.rotation = rotation * targetObject.rotation;
                previousMousePosition = Input.mousePosition;
            }

            // Notas → traducir
            if (!isPickable && Input.GetKeyDown(KeyCode.E) && translateText != null)
            {
                bool isActive = translateText.gameObject.activeSelf;
                translateText.gameObject.SetActive(!isActive);
                if (!isActive) translateText.text = noteText ?? string.Empty;
            }

            // Objetos recogibles → coger (Esto no debería ir aquí, pero queda funcional.)
            if (isPickable && Input.GetKeyDown(KeyCode.E) && heldObject == null)
            {            
                heldObject = targetObject;
                pickObj = heldObject.GetComponent<PickableObject>();
                pickObj.HoldItem(heldObject);
                ExitInspection();
                targetObject = null;
            }
            
            // Click derecho → salir
            if (Input.GetMouseButtonDown(1))
                ExitInspection();
        } // Esto tampoco
        if (pickObj != null && pickObj.IsFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            linternaPro.Instance?.ToggleFlashlight(pickObj.flashlightLight);
        }
        // Dropear item ( Ni esto )
        if (Input.GetKeyDown(KeyCode.G) && heldObject != null)
        {
            pickObj = heldObject.GetComponent<PickableObject>();
            pickObj.DropItem(HeldObject);
            isPickable = false;
            heldObject = null;
        }
    }

    private void DropHeldObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        Collider[] colliders = heldObject.GetComponents<Collider>();

        heldObject.SetParent(null);

        foreach (Collider c in colliders)
        {
            if (!c.isTrigger) c.enabled = true;
        }

        Vector3 dropPos = Camera.main.transform.position + Camera.main.transform.forward * 1f + Vector3.up * 0.5f;
        if (Physics.Raycast(dropPos, Vector3.down, out RaycastHit hit, 5f))
            dropPos.y = hit.point.y + 0.05f;
        heldObject.position = dropPos;

        if (rb != null) StartCoroutine(EnablePhysicsNextFrame(rb));

        heldObject = null;
        UIManager.Instance?.HideControlsMessage();
    }
    
    private IEnumerator EnablePhysicsNextFrame(Rigidbody rb)
    {
        yield return null;
        if (rb != null)
        {
            rb.isKinematic = false; // Activar física dinámica.
        }
    }

    public void ExitInspection()
    {
        if (!IsInspecting) return;

        if (!isPickable || (isPickable && heldObject == null))
        {
            targetObject.SetParent(originalParent);
            targetObject.position = originalPos;
            targetObject.rotation = originalRot;
        }

        translateText?.gameObject.SetActive(false);
        inspectionPanel?.SetActive(false);

        playerFreezer?.Unfreeze();
        UIManager.Instance?.ShowNormalCrosshair();

        if (!isPickable)
            targetObject = null;

        noteText = null;
        Cursor.visible = false;
        IsInspecting = false;
    }
}
