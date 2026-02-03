using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    public Transform PlayerCam, MirrorCam;

    // Actualiza la cámara del espejo en función de la cámara del jugador.
    void Update()
    {
        if (PlayerCam == null || MirrorCam == null) return;

        Vector3 PosY = new Vector3(transform.position.x, PlayerCam.transform.position.y, transform.position.z);
        Vector3 side1 = PlayerCam.transform.position - PosY;
        Vector3 side2 = transform.forward;
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);

        MirrorCam.localEulerAngles = new Vector3(0, angle, 0);
    }
}
