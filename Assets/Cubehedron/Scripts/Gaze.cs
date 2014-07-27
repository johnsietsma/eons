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

    void Start()
    {
        UpdateCamera();
        OVRMessenger.AddListener<OVRMainMenu.Device, bool>( "Sensor_Attached", UpdateDeviceDetectionMsgCallback );
        Screen.showCursor = !hideMousePointer;
    }

    void Update ()
    {
        RaycastHit hit;
        GameObject newCurrentGazeObject;

        if ( Physics.Raycast( GazeTransform.position, GazeTransform.forward, out hit, Mathf.Infinity, gazeLayerMask  ) ) {
            CurrentGazeHit = hit;
            newCurrentGazeObject = hit.transform.gameObject;

        }
        else {
            CurrentGazeHit = new RaycastHit();
            newCurrentGazeObject = null;
        }

        bool newGazeObject = CurrentGazeObject != newCurrentGazeObject;

        var gazeHit = new GazeHit() {
            gaze = this,
            hit = hit
        };

        // Exit the current gaze object
        if ( newGazeObject && CurrentGazeObject != null ) {
            CurrentGazeObject.SendMessage( GazeExitMessage, gazeHit, SendMessageOptions.DontRequireReceiver );
            if ( debug ) { D.Log( "GazeExit: {0}", CurrentGazeObject.name ); }
        }

        CurrentGazeObject = newCurrentGazeObject;

        if ( CurrentGazeObject != null ) {
            CurrentGazeObject.SendMessage( GazeStayMessage, gazeHit, SendMessageOptions.DontRequireReceiver );
            if ( debug ) { D.Log( "GazeStay: {0}", CurrentGazeObject.name ); }
        }

        // Switch to the new object
        if ( newGazeObject && CurrentGazeObject != null ) {
            // Enter the new gaze object
            CurrentGazeObject.SendMessage( GazeEnterMessage, gazeHit, SendMessageOptions.DontRequireReceiver );
            if ( debug ) { D.Log( "GazeEnter: {0}", CurrentGazeObject.name ); }
        }
    }

    void OnGizmosSelected()
    {
        Gizmos.DrawRay( GazeTransform.position, GazeTransform.forward );
    }

    void UpdateDeviceDetectionMsgCallback( OVRMainMenu.Device device, bool attached )
    {
        UpdateCamera();
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
