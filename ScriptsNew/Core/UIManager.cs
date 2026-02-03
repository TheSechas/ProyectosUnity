using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject crosshairNormal;
    [SerializeField] private GameObject crosshairHand;
    [SerializeField] private TextMeshProUGUI interactionText;

    // Singleton sencillo para acceso global desde otros scripts.
    void Awake() => Instance = this;

    // Mostrar texto de interacción (abrir puerta, coger nota, etc.)
    public void ShowInteraction(string text, bool isInteractable)
    {
        if (interactionText == null) return;

        interactionText.text = text;
        interactionText.gameObject.SetActive(!string.IsNullOrEmpty(text));

        crosshairNormal?.SetActive(!isInteractable);
        crosshairHand?.SetActive(isInteractable);
    }

    public void HideAllCrosshairs()
    {
        crosshairNormal?.SetActive(false);
        crosshairHand?.SetActive(false);
    }

    public void ShowNormalCrosshair()
    {
        crosshairNormal?.SetActive(true);
    }

    public void HideInteractionText()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    // --- NUEVOS MÉTODOS ---
    // Usamos el mismo interactionText para controles o mensajes
    public void ShowControlsMessage(string message)
    {
        if (interactionText == null) return;

        interactionText.text = message;
        interactionText.gameObject.SetActive(true);
    }

    public void HideControlsMessage()
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(false);
    }
}
