using UnityEngine;
using System.Collections;

public class FreeLook : MonoBehaviour
{

    private Camera cam;
    private Quaternion startRot;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        startRot = cam.transform.rotation;
    }


    void Update ()
    {
        cam.transform.rotation = startRot;
        cam.fieldOfView = 90;
        var screenPos = Input.mousePosition;
        screenPos.z = cam.nearClipPlane;
        Vector3 p = cam.ScreenToWorldPoint( screenPos );
        cam.fieldOfView = 60;

        transform.LookAt( p );

    }
}
