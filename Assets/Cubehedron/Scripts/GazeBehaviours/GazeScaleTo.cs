using UnityEngine;
using System.Collections;

public class GazeScaleTo : GazeBehaviour
{
    [SerializeField] private Vector3 scale;
    [SerializeField] private float speed;
    [SerializeField] private Space space;
    [SerializeField] private iTween.EaseType easeType;
    [SerializeField] private GameObject scaleObject;

    private bool hasGaze;
    private Vector3 startScale;

    void Awake()
    {
        if ( scaleObject == null ) { scaleObject = gameObject; }
        startScale = scaleObject.transform.localScale;

    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        iTween.ScaleTo( scaleObject, iTween.Hash(
                            "name", "gazeScaleTo",
                            "scale", scale,
                            "speed", speed
                        )
                      );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.ScaleTo( scaleObject, iTween.Hash(
                            "scale", startScale,
                            "speed", speed
                        )
                      );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        iTween.StopByName( "gazeScaleTo" );
    }
}
