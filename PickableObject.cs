using UnityEngine;
public class PickableObject : MonoBehaviour
{
    public bool isPickable = true;

    private void OnTiggerEnter(Collider other)
    {
        if (other.tag == "PlayerInteractionZone")
        {
            other.GetComponentInParent<PickUpObjects>().ObjectToPickUp = this.gameObject;
        }
    }

    private void OnTiggerExit(Collider other)
    {
        if (other.tag == "PlayerInteractionZone")
        {
            other.GetComponentInParent<PickUpObjects>().ObjectToPickUp = null;
        }
    }
}
