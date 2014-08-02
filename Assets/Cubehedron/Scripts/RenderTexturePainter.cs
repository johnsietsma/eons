using UnityEngine;
using System.Collections.Generic;

public class RenderTexturePainter : MonoBehaviour
{
    [SerializeField] private Texture2D brushTex;
    [SerializeField] private Vector2 brushUVSize;
    [SerializeField] private Material brushMaterial;
    [SerializeField] private string shaderTexturePropertyName = "_MainTex";

    [HideInInspector]
    public List<Vector3> positionsUV;

    private RenderTextureBliter renderTextureBilter;
    private Vector2 brushSize;
    private Vector2 halfBrushSize;

    void Start( )
    {
        var bounds = GetBounds();
        Vector2 renderTextureSize = new Vector2( 256, 256 );

        if ( brushUVSize == Vector2.zero ) {
            var pixelSize = GetPixelSize();
            brushUVSize = pixelSize;
            brushUVSize.Scale( renderTextureSize );
        }

        renderTextureBilter = new RenderTextureBliter( renderer.material, shaderTexturePropertyName, brushUVSize, renderTextureSize, bounds, brushMaterial );
    }

    private Bounds GetBounds()
    {
        Bounds? b;

        b = GetComponentInfo<Renderer, Bounds>( c => c.bounds );
        if ( b != null ) { return b.Value; }

#if TK2D
        b = GetComponentInfo<tk2dSprite, Bounds>( c => c.GetBounds() );
        if ( b != null ) { return b.Value; }
#endif

        b = GetComponentInfo<SpriteRenderer, Bounds>( c => c.sprite.bounds );
        if ( b != null ) { return b.Value; }

        Debug.LogError( "Could not find the bounds on which to paint" );
        return new Bounds();
    }

    private Vector2 GetPixelSize()
    {
        Vector2? v;

        v = GetComponentInfo<Renderer, Vector2>( c => new Vector2( c.material.mainTexture.width, c.material.mainTexture.height ) );
        if ( v != null ) { return v.Value; }

#if TK2D
        v = GetComponentInfo<tk2dSprite, Vector2>( c => c.scale );
        if ( v != null ) { return v.Value; }
#endif

        v = GetComponentInfo<SpriteRenderer, Vector2>( c => new Vector2( c.sprite.rect.width, c.sprite.rect.height ) );
        if ( v != null ) { return v.Value; }

        Debug.LogError( "Could not find the pixel size on which to paint" );
        return Vector2.zero;
    }

    private System.Nullable<TRet> GetComponentInfo<T, TRet>( System.Func<T, TRet> func ) where T : Component where TRet: struct {
        var c = GetComponent<T>();
        if ( c )
        {
            return func( c );
        }
        return null;
    }

    void OnRenderObject()
    {
        if ( positionsUV.Count == 0 ) { return; }

        foreach ( Vector3 pos in positionsUV ) {
            renderTextureBilter.BlitUV( pos );
        }
        positionsUV.Clear();
    }
}
