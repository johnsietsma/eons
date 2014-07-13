using UnityEngine;

using HutongGames.PlayMaker;

/**
 * Receives gaze events and forwards them to any PlayMaker FSMs on the this game object.
 */
public class GazePlayMakerEvents : MonoBehaviour
{
    [UnityEngine.Tooltip( "Event to send when receiving a gaze enter event." )]
    public string gazeEnterEvent = "OnGazeEnter";

    [UnityEngine.Tooltip( "Event to send when receiving a gaze exit event." )]
    public string gazeExitEvent = "OnGazeExit";

    public PlayMakerFSM[] playMakerFSMs;

    public void Awake()
    {
        if ( playMakerFSMs == null || playMakerFSMs.Length == 0 ) {
            playMakerFSMs = GetComponents<PlayMakerFSM>();
        }
    }

    public void OnGazeEnter( Gaze gaze )
    {
        foreach ( var fsm in playMakerFSMs ) {
            fsm.Fsm.Event( gazeEnterEvent );
        }
    }

    public void OnGazeExit( Gaze gaze )
    {
        foreach ( var fsm in playMakerFSMs ) {
            fsm.Fsm.Event( gazeExitEvent );
        }
    }
}

