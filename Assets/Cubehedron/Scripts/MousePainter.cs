using UnityEngine;
using System.Collections.Generic;

[RequireComponent( typeof( RenderTexturePainter ) )]
public class MousePainter : MonoBehaviour
{
    private RenderTexturePainter painter;

    void Awake()
    {
        painter = GetComponent<RenderTexturePainter>();
    }

    void Update()
    {
        if ( Input.GetMouseButton( 0 ) ) {
            var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            foreach ( var hit in Physics.RaycastAll( ray ) ) {
                if ( hit.transform == transform ) {
                    painter.positionsUV.Add( hit.textureCoord );
                }
            }

        }
    }
}
