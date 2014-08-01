using UnityEngine;
using System.Collections;

public class GazeRotateBy : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [SerializeField] private Space space;

    private bool hasGaze;

    void Update()
    {
        if ( hasGaze ) {
            var r = rotation * Time.deltaTime * speed;
            D.Log( "Rot: " + r );
            transform.Rotate( r, space );
        }
    }

    public void OnGazeEnter( GazeHit hit )
    {
        hasGaze = true;
    }

    public void OnGazeExit( GazeHit hit )
    {
        hasGaze = false;
    }
}
