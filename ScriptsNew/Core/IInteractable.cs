// Contrato común para cualquier objeto interactuable del juego.
public interface IInteractable
{
    // Texto que se muestra en la UI cuando el jugador apunta al objeto.
    string GetInteractionText(); // <- usa este nombre (igual que en Interactor)
    void Interact();
}

