using UnityEngine;
using System.Collections;

public class GazeShakeRotation : MonoBehaviour
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

    public void OnGazeEnter( GazeHit hit )
    {
        iTween.ShakeRotation( shakeObject, iTween.Hash(
                            "amount", amount,
                            "speed", speed,
                            "loopType", loopType
                        )
                      );
    }

    public void OnGazeExit( GazeHit hit )
    {
        iTween.Stop( "shake" );
    }
}
