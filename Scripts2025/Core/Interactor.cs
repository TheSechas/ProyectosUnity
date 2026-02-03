using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactLayer;

    private IInteractable currentInteractable;

    void Update()
    {
        if (InspectionSystem.Instance != null && InspectionSystem.Instance.IsInspecting)
        {
            UIManager.Instance.HideAllCrosshairs();
            UIManager.Instance.HideInteractionText();
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                UIManager.Instance.ShowInteraction(currentInteractable.GetInteractionText(), true);

                // Diferenciar tipos
                if (currentInteractable is Note || currentInteractable is PickableObject)
                {
                    if (Input.GetMouseButtonDown(0))
                        currentInteractable.Interact();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                        currentInteractable.Interact();
                }

                return;
            }
        }

        currentInteractable = null;
        UIManager.Instance.ShowInteraction("", false);
    }

}
