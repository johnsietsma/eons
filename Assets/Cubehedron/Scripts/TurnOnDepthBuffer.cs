using UnityEngine;
using System.Collections;

public class TurnOnDepthBuffer : MonoBehaviour
{
    void Start ()
    {
        camera.depthTextureMode |= DepthTextureMode.Depth;
    }
}
