using UnityEngine;
using System.Collections;

public class GazeUVShifter : MonoBehaviour
{
    [SerializeField] private Material offsetMaterial;
    [SerializeField] private string texturePropertyName = "_MainTex";
    [SerializeField] private Vector2 uvOffsetPerSecond;

    private Vector2 startOffset;
    private Vector2 currentOffset;
    private bool hasGaze = false;

    void Awake()
    {
        currentOffset = offsetMaterial.mainTextureOffset;
        startOffset = currentOffset;
    }

    void OnDestroy()
    {
        offsetMaterial.mainTextureOffset = startOffset; // Put it back like we found it, stop asset changes.
    }

    void Update()
    {
        if( hasGaze ) {
            currentOffset += uvOffsetPerSecond * Time.deltaTime;
            offsetMaterial.SetTextureOffset( texturePropertyName, currentOffset );
        }
    }

    public void OnGazeEnter( GazeHit hit )
    {
        hasGaze = true;
    }

    public void OnGazeExit( GazeHit hit )
    {
        hasGaze = false;
    }
}
