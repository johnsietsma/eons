using UnityEngine;
using System.Collections;

public class GazeExamine : MonoBehaviour
{
    [SerializeField] private float examineRadius;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 examineOffset;

    private Vector3 startPos;
    private GazeInput gazeInput;

    void Awake()
    {
        startPos = transform.position;
    }

    public void OnGazeEnter( GazeHit hit )
    {
        if ( !enabled ) { return; }
        gazeInput = hit.gazeInput;
        var pos = gazeInput.GazeTransform.forward * examineRadius;

        iTween.MoveTo( gameObject, iTween.Hash(
                           "position", gazeInput.GazeTransform.position + pos + examineOffset,
                           "speed", speed,
                           "space", Space.World
                       ) );
    }

    public void OnGazeExit( GazeHit hit )
    {
        if ( !enabled ) { return; }

        iTween.MoveTo( gameObject, iTween.Hash(
                           "position", startPos,
                           "speed", speed,
                           "space", Space.World
                       ) );

        gazeInput = null;
    }
}
