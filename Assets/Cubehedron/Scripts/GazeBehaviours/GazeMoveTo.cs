using UnityEngine;
using System.Collections;

public class GazeMoveTo : GazeBehaviour
{
    [SerializeField] GameObject moveObject;
    [SerializeField] private Vector3 position;
    [SerializeField] private float speed;
    [SerializeField] private Space space = Space.Self;

    private Vector3 startPosition;
    private string tweenName;

    void Awake()
    {
        if( moveObject==null ) moveObject = gameObject;
        startPosition = moveObject.transform.localPosition;
        tweenName = "GazeMoveTo: " + moveObject.name;
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        D.Log( "Move {0} to {1}", tweenName, position );
        //iTween.StopByName( tweenName );
        //if( space==Space.Self ) moveObject.transform.localPosition = startPosition;
        //else  moveObject.transform.position = startPosition;
        iTween.MoveTo( moveObject, iTween.Hash(
            "name", tweenName,
            "position", position,
            "speed", speed
            //"space", space,
            //"islocal", space==Space.Self
            ));
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        D.Log( "Move {0} back to {1}", tweenName, startPosition );
        iTween.MoveTo( moveObject, iTween.Hash(
            "name", tweenName,
            "position", startPosition,
            "speed", speed,
            "space", space,
            "islocal", true
            ));
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        //iTween.Stop( tweenName );
    }
}
