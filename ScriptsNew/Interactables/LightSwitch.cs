using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light targetLight;

    public string GetInteractionText() =>
        targetLight.enabled ? "Pulsa E para apagar la luz" : "Pulsa E para encender la luz";

    public void Interact()
    {
        if (targetLight) targetLight.enabled = !targetLight.enabled;
    }
}
