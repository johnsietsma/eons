using UnityEngine;
using System.Collections;

public class GazeRotateBy : GazeBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private Space space;

    protected override void DoGazeEnter( GazeHit hit )
    {
        iTween.RotateBy( gameObject, iTween.Hash(
            "amount", rotation,
            "speed", speed,
            "space", space,
            "islocal", true
            ));
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.Stop( "rotate" );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        iTween.Stop( "rotate" );
    }
}
