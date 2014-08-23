using UnityEngine;
using System.Collections;

public class GazeExamine : MonoBehaviour
{
    [SerializeField] private Gaze gaze;
    [SerializeField] private float examineRadius;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 examineOffset;

    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

    void Update() {}

    public void OnGazeEnter( GazeHit hit )
    {
        if ( !enabled ) { return; }
        var pos = gaze.GazeTransform.forward * examineRadius;

        iTween.MoveTo( gameObject, iTween.Hash(
                           "position", gaze.GazeTransform.position + pos + examineOffset,
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
    }
}
