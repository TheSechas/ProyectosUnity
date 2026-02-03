using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private HashSet<string> keys = new HashSet<string>(); // llaves recogidas
    private HashSet<string> unlockedDoors = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Llamar cuando recojas una llave
    public void PickUpKey(string doorID)
    {
        keys.Add(doorID);
    }

    public bool HasKey(string doorID)
    {
        return keys.Contains(doorID);
    }

    public void UnlockDoor(string doorID)
    {
        unlockedDoors.Add(doorID);
    }

    public bool IsDoorUnlocked(string doorID)
    {
        return unlockedDoors.Contains(doorID);
    }
}
