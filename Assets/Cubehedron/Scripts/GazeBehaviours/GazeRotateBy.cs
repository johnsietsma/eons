using UnityEngine;
using System.Collections;

public class GazeRotateBy : GazeBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;

    private Quaternion startRotation;

    void Awake()
    {
        startRotation = transform.localRotation;
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        iTween.Stop( "rotate" );
        transform.localRotation = startRotation;
        iTween.RotateTo( gameObject, iTween.Hash(
            "rotation", rotation,
            "speed", speed,
            "space", Space.Self,
            "islocal", true
            ));
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.Stop( "rotate" );
        iTween.RotateTo( gameObject, iTween.Hash(
            "rotation", startRotation.eulerAngles,
            "speed", speed,
            "space", Space.Self,
            "islocal", true
            ));
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        iTween.Stop( "rotate" );
    }
}
