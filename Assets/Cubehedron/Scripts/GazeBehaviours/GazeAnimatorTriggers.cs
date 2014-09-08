using UnityEngine;
using System.Collections;

public class GazeAnimatorTriggers : GazeBehaviour
{
    [Tooltip( "The animator to set the triggers on. If not sute, use the Animator on this object." )]
    [SerializeField] private Animator gazeAnimator;
    [SerializeField] private string GazeEnterTrigger = GazeInput.GazeEnterMessage;
    [SerializeField] private string GazeExitTrigger = GazeInput.GazeExitMessage;

    private int startStateHash;

    void Awake()
    {
        if ( gazeAnimator == null ) { gazeAnimator = GetComponent<Animator>(); }
        D.Assert( gazeAnimator, "Gaze animator hasn't been set or there is no Animator on this object." );

        var stateInfo = gazeAnimator.GetCurrentAnimatorStateInfo( 0 );
        startStateHash = stateInfo.nameHash;
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        gazeAnimator.SetTrigger( GazeEnterTrigger );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        gazeAnimator.SetTrigger( GazeExitTrigger );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        gazeAnimator.ResetTrigger( GazeEnterTrigger );
        gazeAnimator.ResetTrigger( GazeExitTrigger );
        gazeAnimator.Play( startStateHash );
    }

}
