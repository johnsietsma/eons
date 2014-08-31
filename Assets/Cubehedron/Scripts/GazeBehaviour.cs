using UnityEngine;
using System.Collections;

public abstract class GazeBehaviour : MonoBehaviour
{
    public enum ExitBehaviour { Nothing, Stop, Undo };

    [SerializeField] ExitBehaviour exitBehaviour;
    [SerializeField] float activateDelay;
    [SerializeField] bool debug;

    private bool isEntered;
    private bool isActivated;

    public void OnGazeEnter( GazeHit hit )
    {
        if ( !enabled || isEntered ) { return; }
        DoGazeEnter( hit );
        isEntered = true;
        Invoke( "DoGazeActivate", activateDelay );
    }

    public void OnGazeStay( GazeHit hit )
    {
        if ( !enabled ) { return; }
        DoGazeStay( hit );
    }

    public void OnGazeExit( GazeHit hit )
    {
        if ( !enabled ) { return; }
        DoGazeExitInternal( hit );
    }

    protected virtual void DoGazeEnterInternal( GazeHit hit )
    {
        if ( debug ) { D.Log( "DoGazeEnter({0})", hit ); }
        DoGazeEnter( hit );
        isEntered = true;
    }

    protected virtual void DoGazeStayInternal( GazeHit hit )
    {
        if ( debug ) { D.Log( "DoGazeStay({0})", hit ); }
        DoGazeStay( hit );
    }

    protected virtual void DoGazeExitInternal( GazeHit hit )
    {
        if ( debug ) { D.Log( "DoGazeExit({0})", hit ); }

        if ( exitBehaviour == ExitBehaviour.Stop ) { DoGazeStop( hit ); }
        if ( exitBehaviour == ExitBehaviour.Undo ) { DoGazeExit( hit ); }
        isEntered = false;
        if ( !isActivated ) { CancelInvoke( "DoGazeActivate" ); }
    }

    protected virtual void DoGazeStopInternal( GazeHit hit )
    {
        if ( debug ) { D.Log( "DoGazeStop({0})", hit ); }
        DoGazeStop( hit );
        isEntered = false;
        if ( !isActivated ) { CancelInvoke( "DoGazeActivate" ); }
    }

    protected virtual void DoGazeActivateInternal()
    {
        if ( debug ) { D.Log( "DoGazeActivate()" ); }
        DoGazeActivate();
        isActivated = true;
    }

    protected virtual void DoGazeDeactivateInternal()
    {
        if ( debug ) { D.Log( "DoGazeDeactivate()" ); }
        DoGazeActivate();
        isActivated = false;
    }

    protected virtual void DoGazeEnter( GazeHit hit ) {}
    protected virtual void DoGazeStay( GazeHit hit ) {}
    protected virtual void DoGazeExit( GazeHit hit ) {}
    protected virtual void DoGazeStop( GazeHit hit ) {}
    protected virtual void DoGazeActivate() {}
    protected virtual void DoGazeDeactivate() {}
}
