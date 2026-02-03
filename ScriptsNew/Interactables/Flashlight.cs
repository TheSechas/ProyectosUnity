using UnityEngine;

public class linternaPro : MonoBehaviour
{
    public static linternaPro Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Enciende/apaga la luz de la linterna que tengamos en la mano.
    public void ToggleFlashlight(Light targetLight)
    {
        if (targetLight == null)
        {
            Debug.LogWarning("ToggleFlashlight: targetLight es null");
            return;
        }

        // Si el GameObject del Light está inactivo, lo activamos (opcional)
        if (!targetLight.gameObject.activeInHierarchy)
            targetLight.gameObject.SetActive(true);

        // Toggle directo sobre el componente Light
        targetLight.enabled = !targetLight.enabled;
        Debug.Log($"ToggleFlashlight: {targetLight.name} -> {(targetLight.enabled ? "ON" : "OFF")}");

    }
}
