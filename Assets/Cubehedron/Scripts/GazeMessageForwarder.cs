using UnityEngine;
using System.Collections;

/**
 * Forward messages from the GazeInput to another object.
 */
public class GazeMessageForwarder : MonoBehaviour
{
    [SerializeField] private GameObject messageReceiver;
    [SerializeField] private string GazeEnterMessage = GazeInput.GazeEnterMessage;
    [SerializeField] private string GazeExitMessage = GazeInput.GazeExitMessage;

    void Awake()
    {
        D.Assert( messageReceiver, "Gaze message receiver hasn't been set." );
    }

    public void OnGazeEnter( GazeHit hit )
    {
        D.Log( "Forward OnGazeEnter to {0}.", messageReceiver.name );
        messageReceiver.SendMessage( GazeEnterMessage, hit, SendMessageOptions.DontRequireReceiver );
    }

    public void OnGazeExit( GazeHit hit )
    {
        D.Log( "Forward OnGazeExit to {0}.", messageReceiver.name );
        messageReceiver.SendMessage( GazeExitMessage, hit, SendMessageOptions.DontRequireReceiver );
    }
}
