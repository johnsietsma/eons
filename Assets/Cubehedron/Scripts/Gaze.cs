using UnityEngine;
using System.Collections;


/**
 * Mimics the gaze of of an object.
 * Provides events for gazing at objects and hit information about the gaze.
 */
public class Gaze : MonoBehaviour
{
    public static readonly string GazeEnterMessage = "OnGazeEnter";
    public static readonly string GazeExitMessage = "OnGazeExit";

    // Information about the gaze
    public RaycastHit CurrentGazeHit { get; private set; }
    public GameObject CurrentGazeObject { get; private set; }
    public Transform GazeTransform { get { return gazeTransform; } }

    [Tooltip( "The Transform the gaze originates from." )]
    [SerializeField] private Transform gazeTransform;

    [Tooltip( "The layers the gaze will hit" )]
    [SerializeField] private LayerMask gazeLayerMask;

    [SerializeField] private bool debug;

    void Awake()
    {
        if ( gazeTransform == null ) {
            // Grab the main camera if the game transform hasn't been set or is disabled.
            // This is useful when the Oculus camera is turned off.
            gazeTransform = Camera.main.transform;
        }
    }

    void Update ()
    {
        RaycastHit hit;
        GameObject newCurrentGazeObject;

        if ( Physics.Raycast( gazeTransform.position, gazeTransform.forward, out hit, Mathf.Infinity, gazeLayerMask  ) ) {
            CurrentGazeHit = hit;
            newCurrentGazeObject = hit.transform.gameObject;

        }
        else {
            CurrentGazeHit = new RaycastHit();
            newCurrentGazeObject = null;
        }

        if ( CurrentGazeObject != newCurrentGazeObject ) {
            // Exit the current gaze object
            if ( CurrentGazeObject != null ) {
                CurrentGazeObject.SendMessage( GazeExitMessage, this, SendMessageOptions.DontRequireReceiver );
                if ( debug ) { D.Log( "GazeExit: {0}", CurrentGazeObject.name ); }
            }

            // Switch to the new object
            CurrentGazeObject = newCurrentGazeObject;

            // Enter the new gaze object
            CurrentGazeObject.SendMessage( GazeEnterMessage, this, SendMessageOptions.DontRequireReceiver );
            if ( debug ) { D.Log( "GazeEnter: {0}", CurrentGazeObject.name ); }
        }
    }

    void OnGizmosSelected()
    {
        Gizmos.DrawRay( gazeTransform.position, gazeTransform.forward );
    }
}
