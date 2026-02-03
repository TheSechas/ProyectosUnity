using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private string doorID; // ⚠️ Esto faltaba

    public string GetInteractionText() => "Pulsa E para recoger llave";

    public void Interact()
    {
        if (!string.IsNullOrEmpty(doorID))
            GameManager.Instance.PickUpKey(doorID); // registrar la llave para esa puerta

        Destroy(gameObject); // la llave desaparece al recogerla
    }
}
