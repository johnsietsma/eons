using UnityEngine;
using System.Collections;

/**
 * For providing feedback on the gaze.
 */
public class GazeGuidance : MonoBehaviour
{
    [Tooltip( "Optional: Find a gaze object in the scene if not set." )]
    [SerializeField] private Gaze gaze;

    [SerializeField] private GameObject gazePointer;

    void Awake()
    {
        if ( gaze == null ) {
            gaze = FindObjectOfType( typeof( Gaze ) ) as Gaze;
        }

        D.Assert( gazePointer, "Please set the gaze pointer" );
    }

    void Update ()
    {
        if ( gaze.CurrentCamera == null ) { return; }

        var gazeCamTransform = gaze.CurrentCamera.transform;
        transform.position = gazeCamTransform.position;
        transform.rotation = gazeCamTransform.rotation;

        if ( gaze.CurrentGazeHit.collider != null ) {
            if ( !gazePointer.activeInHierarchy ) {
                gazePointer.SetActive( true );
            }
            gazePointer.transform.position = gaze.CurrentGazeHit.point;
        }
        else {
            if ( gazePointer.activeInHierarchy ) {
                gazePointer.SetActive( false );
            }
        }
    }
}
