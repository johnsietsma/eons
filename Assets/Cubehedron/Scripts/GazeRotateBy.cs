using UnityEngine;
using System.Collections;

public class GazeRotateBy : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private Space space;

    public void OnGazeEnter( GazeHit hit )
    {
        iTween.RotateBy( gameObject, iTween.Hash(
            "amount", rotation,
            "speed", speed,
            "space", space,
            "islocal", true
            ));
    }

    public void OnGazeExit( GazeHit hit )
    {
        iTween.Stop( "rotate" );
    }
}
