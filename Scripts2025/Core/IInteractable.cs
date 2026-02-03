using NUnit.Framework;
using UnityEngine;

public interface IInteractable
{
    string GetInteractionText(); // <- usa este nombre (igual que en Interactor)
    void Interact();
}

