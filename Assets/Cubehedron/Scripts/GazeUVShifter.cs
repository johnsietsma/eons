using UnityEngine;
using System.Collections;

public class GazeUVShifter : GazeBehaviour
{
    [SerializeField] private Material offsetMaterial;
    [SerializeField] private string texturePropertyName = "_MainTex";
    [SerializeField] private Vector2 uvOffsetPerSecond;

    private Vector2 startOffset;
    private Vector2 currentOffset;
    private bool hasGaze = false;

    void Awake()
    {
        currentOffset = offsetMaterial.GetTextureOffset( texturePropertyName );
        startOffset = currentOffset;
    }

    void OnDestroy()
    {
        offsetMaterial.SetTextureOffset( texturePropertyName, startOffset ); // Put it back like we found it, stop asset changes.
    }

    void Update()
    {
        if( hasGaze ) {
            currentOffset += uvOffsetPerSecond * Time.deltaTime;
            offsetMaterial.SetTextureOffset( texturePropertyName, currentOffset );
        }
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        hasGaze = true;
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        hasGaze = false;
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        hasGaze = false;
    }
}
