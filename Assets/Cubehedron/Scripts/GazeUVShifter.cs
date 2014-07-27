using UnityEngine;
using System.Collections;

public class GazeUVShifter : MonoBehaviour
{
    [SerializeField] private Material offsetMaterial;
    [SerializeField] private Vector2 uvOffsetPerSecond;

    private Vector2 currentOffset;
    private bool hasGaze = false;

    void Awake()
    {
        currentOffset = offsetMaterial.mainTextureOffset;
    }

    void Update()
    {
        if( hasGaze ) {
            currentOffset += uvOffsetPerSecond * Time.deltaTime;
            offsetMaterial.mainTextureOffset = currentOffset;
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
