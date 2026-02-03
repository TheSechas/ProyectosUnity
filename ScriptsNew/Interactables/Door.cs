using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 6f;
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private string doorID;

    private bool isOpen;
    private bool unlocked;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Awake()
    {
        if (doorPivot == null) doorPivot = transform;
        closedRot = doorPivot.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;
    }

    void Update()
    {
        Quaternion targetRot = isOpen ? openRot : closedRot;
        doorPivot.localRotation = Quaternion.Lerp(doorPivot.localRotation, targetRot, Time.deltaTime * speed);
    }

    public string GetInteractionText()
    {
        if (requiresKey && !unlocked && !HasKeyInHand())
            return "Necesitas una llave";

        return isOpen ? "Pulsa E para cerrar puerta" : "Pulsa E para abrir puerta";
    }

    public void Interact()
    {
        if (requiresKey && !unlocked)
        {
            if (HasKeyInHand())
            {
                unlocked = true;  // desbloqueo permanente
                isOpen = true;    // abre al instante
            }
            else
            {
                UIManager.Instance.ShowInteraction("Necesitas una llave", true);
            }
            return;
        }

        isOpen = !isOpen;
    }

    private bool HasKeyInHand()
    {
        if (InspectionSystem.Instance.HeldObject == null)
            return false;

        PickableObject held = InspectionSystem.Instance.HeldObject.GetComponent<PickableObject>();
        return held != null && held.DoorID == doorID;
    }
}
