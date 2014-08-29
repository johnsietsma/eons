using UnityEngine;
using System.Collections;

public abstract class GazeBehaviour : MonoBehaviour
{
    public bool undoOnExit;

    private bool isEntered;

    public void OnGazeEnter( GazeHit hit )
    {
        if( !enabled || isEntered ) return;
        DoGazeEnter( hit );
        isEntered = true;
    }

    public void OnGazeStay( GazeHit hit )
    {
        if( !enabled ) return;
        DoGazeStay( hit );
    }

    public void OnGazeExit( GazeHit hit )
    {
        if( !enabled ) return;
        if ( undoOnExit ) {
            D.Log( "Exit: {0} {1}", undoOnExit, name );
            DoGazeExit( hit );
            isEntered = false;
        }
    }

    void Update() {} // Just so we can disable it in the editor

    protected abstract void DoGazeEnter( GazeHit hit );
    protected virtual void DoGazeStay( GazeHit hit ) {}
    protected abstract void DoGazeExit( GazeHit hit );
}
