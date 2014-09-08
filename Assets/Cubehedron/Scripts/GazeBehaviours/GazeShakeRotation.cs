using UnityEngine;
using System.Collections;

public class GazeShakeRotation : GazeBehaviour
{
    [SerializeField] private Vector3 amount;
    [SerializeField] private float speed;
    [SerializeField] private iTween.LoopType loopType;
    [SerializeField] private GameObject shakeObject;
    private bool hasGaze;
    private Vector3 startScale;

    void Awake()
    {
        if ( shakeObject == null ) { shakeObject = gameObject; }
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        iTween.ShakeRotation( shakeObject, iTween.Hash(
                            "name", "gazeShakeRotation",
                            "amount", amount,
                            "speed", speed,
                            "loopType", loopType
                        )
                      );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.Stop( "gazeShakeRotation" );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        iTween.Stop( "gazeShakeRotation" );
    }
}
