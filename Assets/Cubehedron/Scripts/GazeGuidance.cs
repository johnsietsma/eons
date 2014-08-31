using UnityEngine;
using System.Collections;

/**
 * For providing feedback on the gaze.
 */
public class GazeGuidance : MonoBehaviour
{
    [Tooltip( "Optional: Find a gaze object in the scene if not set." )]
    [SerializeField] private GazeInput gazeInput;

    [SerializeField] private GameObject gazePointer;

    void Awake()
    {
        if ( gazeInput == null ) {
            gazeInput = FindObjectOfType( typeof( GazeInput ) ) as GazeInput;
        }

        D.Assert( gazePointer, "Please set the gaze pointer" );
    }

    void Update ()
    {
        if ( gazeInput.CurrentCamera == null ) { return; }

        var gazeCamTransform = gazeInput.CurrentCamera.transform;
        transform.position = gazeCamTransform.position;
        transform.rotation = gazeCamTransform.rotation;

        if ( gazeInput.CurrentGazeHit.collider != null ) {
            if ( !gazePointer.activeInHierarchy ) {
                gazePointer.SetActive( true );
            }
            gazePointer.transform.position = gazeInput.CurrentGazeHit.point;
        }
        else {
            if ( gazePointer.activeInHierarchy ) {
                gazePointer.SetActive( false );
            }
        }
    }
}
