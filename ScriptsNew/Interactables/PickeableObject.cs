using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    public static PickableObject Instance;
    [SerializeField] private string doorID;                 // para llaves
    [SerializeField] private string objectName = "objeto";  // texto UI
    [SerializeField] private bool isFlashlight = false;     // si es linterna
    [SerializeField] public Light flashlightLight;         // referencia al Light de la linterna

    [Header("Hand Hold Point")]
    [SerializeField] private Transform handHoldPoint;

    private bool flashlightOn = false;
    private Rigidbody instanceRigidBody;
    private Collider[] instanceColliders;
    public string DoorID => doorID;
    public bool IsFlashlight => isFlashlight;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void Interact()
    {
        // Empieza la inspeccion, aseguramos de que solo ocurra unos sola vez con "wasInspected"

        InspectionSystem.Instance.StartInspection(transform, null, true);

    }

    public string GetInteractionText()
    {
        return $"Pulsa click izquierdo para inspeccionar";
    }

    public void HoldItem(Transform heldObject)
    {
        heldObject.SetParent(handHoldPoint);
        heldObject.localPosition = Vector3.zero;
        if (IsFlashlight)
        {
            heldObject.localRotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            heldObject.localRotation = Quaternion.identity;
        }

        instanceRigidBody = heldObject.GetComponent<Rigidbody>();
        if (instanceRigidBody != null) instanceRigidBody.isKinematic = true;

        instanceColliders = heldObject.GetComponents<Collider>();
        foreach (Collider c in instanceColliders)
        {
            if (!c.isTrigger) c.enabled = false;
        }

        // Si es llave → registrar en GameManager

        if (!IsFlashlight)
            GameManager.Instance.PickUpKey(DoorID);
    }

    public void DropItem(Transform heldObject)
    {
        heldObject.localPosition += Vector3.forward * 0.3f;
        heldObject.SetParent(null);
        if (instanceRigidBody != null) instanceRigidBody.isKinematic = false; else return;
        foreach (Collider c in instanceColliders)
        {
            if (!c.isTrigger) c.enabled = true;
        }
    }
}