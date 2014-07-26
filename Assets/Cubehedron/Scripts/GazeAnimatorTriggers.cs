using UnityEngine;
using System.Collections;

public class GazeAnimatorTriggers : MonoBehaviour
{
    [Tooltip( "The animator to set the triggers on. If not sute, use the Animator on this object." )]
    [SerializeField] private Animator gazeAnimator;
    [SerializeField] private string GazeEnterTrigger = Gaze.GazeEnterMessage;
    [SerializeField] private string GazeExitTrigger = Gaze.GazeExitMessage;

    void Awake()
    {
        if ( gazeAnimator == null ) { gazeAnimator = GetComponent<Animator>(); }
        D.Assert( gazeAnimator, "Gaze animator hasn't been set or there is no Animator on this object." );
    }

    public void OnGazeEnter( GazeHit hit )
    {
        gazeAnimator.SetTrigger( GazeEnterTrigger );
    }

    public void OnGazeExit( GazeHit hit )
    {
        gazeAnimator.SetTrigger( GazeExitTrigger );
    }
}
