using UnityEngine;
using System.Collections;


/**
 * Blit a material onto a render texture.
 */
public class RenderTextureBliter
{
    private static readonly Color clearColor = new Color( 0, 0, 0, 0 );

    // The RenderTexture we'll be writing to
    private RenderTexture _destinationRenderTexture;

    // The local space bounds of the renderer.
    // Usually renderer.bounds or GetComponent<tk2dSprite>().GetBounds with localScale applied
    private Bounds _rendererBounds;

    // The material we'll be using for drawing
    private Material _brushMaterial;

    // The size we should use for bliting the brush in uv space.
    private Vector2 _brushSize;

    /**
     * @param rendererMaterial. The material to which the RenderTexture will be attached.
     * @param textureName. The name of the texture to attach the RenderTexture to.
     * @param brushSize. The uv size of the brush texture.
     * @param renderTextureSize. The size of the RenderTexture, smaller means more pixelated.
     * @param rendererBounds. The size of the renderer in world units.
     * @param brushMaterial. The material that will be used to paint onto the render texture.
     */
    public RenderTextureBliter( Material rendererMaterial, string textureName, Vector2 brushSize, Vector2 renderTextureSize, Bounds rendererBounds, Material brushMaterial )
    {
        _destinationRenderTexture = new RenderTexture( ( int )renderTextureSize.x, ( int )renderTextureSize.y, 0 );
        _rendererBounds = rendererBounds;
        _brushMaterial = brushMaterial;

        // Render the current material onto the new RenderTexture and then swap the RT in.
        var mat = rendererMaterial;
        Graphics.Blit( mat.mainTexture, _destinationRenderTexture, mat );
        rendererMaterial.SetTexture( textureName, _destinationRenderTexture );

        _brushSize = brushSize;

        Debug.Log( "Brush size: " + _brushSize.ToStringf() );

        ClearTexture( rendererMaterial.GetTexture( textureName ) );
    }

    public void ClearTexture( Texture clearTex )
    {
        D.Assert( clearTex!=null );
        Graphics.Blit( clearTex, _destinationRenderTexture );
    }

    /**
     * Blit the given material onto the Rendertexture.
     * Pos is in the local space of the RenderTexture
     */
    public void BlitPos( Vector2 pos )
    {
        // Move from a centered coord system to a the origin at bottom left.
        pos += ( Vector2 )_rendererBounds.extents;

        // Convert to UV coords
        pos.x /= _rendererBounds.size.x;
        pos.y /= _rendererBounds.size.y;

        // Draw in the centre of the brush
        pos.x -= _brushSize.x * 0.5f;
        pos.y -= _brushSize.y * 0.5f;

        BlitUV( pos );
    }

    /**
      * Blit the given material onto the Rendertexture.
      * uv is uv coords of the blit.
      */
    public void BlitUV( Vector2 uv )
    {
        RenderTexture oldRT = RenderTexture.active;
        RenderTexture.active = _destinationRenderTexture;

        GL.PushMatrix();
        GL.LoadOrtho();

        //D.Log( "Pos:{0} Size:{1}", uv.ToStringf(), _brushSize.ToStringf() );

        for ( var i = 0; i < _brushMaterial.passCount; ++i ) {
            _brushMaterial.SetPass( i );
            DrawQuad( uv, _brushSize );
        }

        GL.PopMatrix();

        RenderTexture.active = oldRT;
    }

    private void DrawQuad( Vector2 pos, Vector2 size )
    {
        float videoDepth = -1f;
        GL.Begin( GL.QUADS );
        GL.TexCoord2( 0.0f, 1.0f );
        GL.Vertex3( pos.x, pos.y + size.y, videoDepth );

        GL.TexCoord2( 1.0f, 1.0f );
        GL.Vertex3( pos.x + size.x, pos.y + size.y, videoDepth );

        GL.TexCoord2( 1.0f, 0.0f );
        GL.Vertex3( pos.x + size.x, pos.y, videoDepth );

        GL.TexCoord2( 0.0f, 0.0f );
        GL.Vertex3( pos.x, pos.y, videoDepth );
        GL.End();
    }

}
