using UnityEngine;
using System.Collections.Generic;

[RequireComponent( typeof( RenderTexturePainter ) )]
public class GazePainter : MonoBehaviour
{
    private RenderTexturePainter painter;

    void Awake()
    {
        painter = GetComponent<RenderTexturePainter>();
    }

    void OnGazeStay( GazeHit hit )
    {
        painter.positionsUV.Add( hit.hit.textureCoord );
    }
}
