using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [TextArea]
    [SerializeField] private string noteText;

    public void Interact()
    {
        // Inspección de la nota (no se puede coger)
        InspectionSystem.Instance?.StartInspection(transform, noteText, false);
    }

    public string GetInteractionText()
    {
        return "Pulsa click izquierdo para examinar";
    }
}
