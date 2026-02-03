using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sens = 3f;

    private float hor;
    private float vert;
    private float xRotation;

    public Transform t_player;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        hor = Input.GetAxis("Mouse X") * sens;
        vert = Input.GetAxis("Mouse Y") * sens;

        xRotation -= vert;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        t_player.Rotate(Vector3.up * hor);
    }
}
