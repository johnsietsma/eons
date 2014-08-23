using UnityEngine;
using System.Collections;


/**
 * Mimics the gaze of of an object.
 * Provides events for gazing at objects and hit information about the gaze.
 */
public class Gaze : MonoBehaviour
{
    public static readonly string GazeEnterMessage = "OnGazeEnter";
    public static readonly string GazeStayMessage = "OnGazeStay";
    public static readonly string GazeExitMessage = "OnGazeExit";

    public Camera CurrentCamera { get; private set; }

    // Information about the gaze
    public RaycastHit CurrentGazeHit { get; private set; }
    public GameObject CurrentGazeObject { get; private set; }
    public Transform GazeTransform { get { return CurrentCamera.transform;  } }

    [Tooltip( "The Rift Camera interface" )]
    [SerializeField] private OVRCameraController ovrCameraController;

    [Tooltip( "The FreeLook component for non-HMD look control." )]
    [SerializeField] private Camera mouseCamera;

    [Tooltip( "The layers the gaze will hit" )]
    [SerializeField] private LayerMask gazeLayerMask;

    [SerializeField] private bool hideMousePointer;
    [SerializeField] private bool debug;

    private Vector3 originalBoundsSize;
    private static readonly RaycastHit DefaultRaycastHit = new RaycastHit();

    void Start()
    {
        UpdateCamera();
        Screen.showCursor = !hideMousePointer;
    }

    void Update ()
    {
        RaycastHit hit;
        GameObject newGazeObject = null;
        RaycastHit newRaycastHit = DefaultRaycastHit;

        if ( Physics.Raycast( GazeTransform.position, GazeTransform.forward, out hit, Mathf.Infinity, gazeLayerMask  ) ) {
            newRaycastHit = hit;
            newGazeObject = hit.transform.gameObject;
        }

        bool isNewGazeObject = CurrentGazeObject != newGazeObject;

        GazeHit gazeHit;
        gazeHit.gaze = this;
        gazeHit.hit = newRaycastHit;

        // Exit the current gaze object
        if ( isNewGazeObject && CurrentGazeObject != null ) {
            DoGazeExit( gazeHit );
        }

        if ( CurrentGazeObject != null ) {
            DoGazeStay( gazeHit );
        }

        // Switch to the new object
        if ( isNewGazeObject && newGazeObject != null ) {
            DoGazeEnter( newGazeObject, gazeHit );
        }
    }

    void OnGizmosSelected()
    {
        Gizmos.DrawRay( GazeTransform.position, GazeTransform.forward );
    }

    private void DoGazeEnter( GameObject newGazeObject, GazeHit gazeHit )
    {
        CurrentGazeObject = newGazeObject;
        CurrentGazeHit = gazeHit.hit;

        BoxCollider bc = CurrentGazeHit.collider as BoxCollider;
        if ( bc != null ) {
            originalBoundsSize = bc.size;
            bc.size = bc.size * 2;
        }

        // Enter the new gaze object
        CurrentGazeObject.SendMessage( GazeEnterMessage, gazeHit, SendMessageOptions.DontRequireReceiver );
        if ( debug ) { D.Log( "GazeEnter: {0}", CurrentGazeObject.name ); }
    }

    private void DoGazeStay( GazeHit gazeHit )
    {
        CurrentGazeObject.SendMessage( GazeStayMessage, gazeHit, SendMessageOptions.DontRequireReceiver );
        if ( debug ) { D.Log( "GazeStay: {0}", CurrentGazeObject.name ); }
    }

    private void DoGazeExit( GazeHit gazeHit )
    {
        CurrentGazeObject.SendMessage( GazeExitMessage, gazeHit, SendMessageOptions.DontRequireReceiver );
        if ( debug ) { D.Log( "GazeExit: {0}", CurrentGazeObject.name ); }

        BoxCollider bc = CurrentGazeHit.collider as BoxCollider;
        if ( bc ) {
            bc.size = originalBoundsSize;
        }

        CurrentGazeObject = null;
        CurrentGazeHit = DefaultRaycastHit;
    }


    private void UpdateCamera()
    {
        if ( OVRDevice.IsHMDPresent() ) {
            ovrCameraController.gameObject.SetActive( true );
            mouseCamera.gameObject.SetActive( false );
            Camera cam = null;
            ovrCameraController.GetCamera( ref cam );
            CurrentCamera = cam;
        }
        else {
            ovrCameraController.gameObject.SetActive( false );
            mouseCamera.gameObject.SetActive( true );
            CurrentCamera = mouseCamera;
        }
    }

}
