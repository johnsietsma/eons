using UnityEngine;
using System.Collections;

[RequireComponent( typeof( GazePickPoint ) )]
public class PropController : MonoBehaviour
{

    private Gaze currentGaze;
    private GazePickPoint gazePickPoint;

    void Awake()
    {
        gazePickPoint = GetComponent<GazePickPoint>();
    }

    public Vector3 GetPickPoint()
    {
        if ( currentGaze == null ) { return Vector3.zero; }
        return gazePickPoint.GetPickPoint( currentGaze );
    }

    public void OnGazeEnter( GazeHit hit )
    {
        currentGaze = hit.gaze;
    }

    public void OnGazeExit( GazeHit hit )
    {
        currentGaze = null;
    }

}
