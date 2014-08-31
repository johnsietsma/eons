using UnityEngine;
using System.Collections;

public abstract class GazeBehaviour : MonoBehaviour
{
    public enum UndoBehaviour { Nothing, Stop, Undo };

    [SerializeField] UndoBehaviour exitBehaviour;
    [SerializeField] float delay;
    [SerializeField] bool debug;

    private bool isEntered;

    // ---- GazeInput messages ----
    public void OnGazeEnter( GazeHit hit )
    {
        if ( !enabled || isEntered ) { return; }
        StartCoroutine( "DoGazeEnterInternalCoroutine", hit );
    }

    public void OnGazeStay( GazeHit hit )
    {
        if ( !enabled ) { return; }
        DoGazeStayInternal( hit );
    }

    public void OnGazeExit( GazeHit hit )
    {
        if ( !enabled ) { return; }
        StopCoroutine( "DoGazeEnterInternalCoroutine" );
        DoGazeExitInternal( hit );
    }

    // ---- Internal handling of GazeInputMessages ----
    protected virtual IEnumerator DoGazeEnterInternalCoroutine( GazeHit hit )
    {
        yield return new WaitForSeconds( delay );
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

        // Decide how to handle the gaze exit.
        if ( exitBehaviour == UndoBehaviour.Stop ) { DoGazeStop( hit ); }
        if ( exitBehaviour == UndoBehaviour.Undo ) { DoGazeExit( hit ); }

        // If there is no exit behaviour we stay "on"
        if ( exitBehaviour != UndoBehaviour.Nothing ) { isEntered = false; }
    }

    protected virtual void DoGazeStopInternal( GazeHit hit )
    {
        if ( debug ) { D.Log( "DoGazeStop({0})", hit ); }
        DoGazeStop( hit );
        isEntered = false;
    }

    protected virtual void DoGazeEnter( GazeHit hit ) {}
    protected virtual void DoGazeStay( GazeHit hit ) {}
    protected virtual void DoGazeExit( GazeHit hit ) {}
    protected virtual void DoGazeStop( GazeHit hit ) {}
}
