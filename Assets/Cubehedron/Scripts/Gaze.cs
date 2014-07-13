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
    public Transform GazeTransform { get { return gazeCamera.transform;  } }

    [Tooltip( "The Rift Camera interface" )]
    [SerializeField] private OVRCameraController ovrCameraController;

    [Tooltip( "The layers the gaze will hit" )]
    [SerializeField] private LayerMask gazeLayerMask;

    [SerializeField] private bool debug;

    private Camera gazeCamera;

    void Start()
    {
        UpdateCamera();
        OVRMessenger.AddListener<OVRMainMenu.Device, bool>( "Sensor_Attached", UpdateDeviceDetectionMsgCallback );
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
        Gizmos.DrawRay( GazeTransform.position, GazeTransform.forward );
    }

    void UpdateDeviceDetectionMsgCallback( OVRMainMenu.Device device, bool attached )
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if ( OVRDevice.IsHMDPresent() ) {
            ovrCameraController.GetCamera( ref gazeCamera );
        }
        else {
            gazeCamera = Camera.main;
        }
    }

}
