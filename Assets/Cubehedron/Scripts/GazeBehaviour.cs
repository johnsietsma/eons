using UnityEngine;
using System.Collections;

public abstract class GazeBehaviour : MonoBehaviour
{
    public bool playBackwardsOnExit;

    public void OnGazeEnter( GazeHit hit )
    {
        DoGazeEnter( hit );
    }

    public void OnGazeStay( GazeHit hit )
    {
        DoGazeStay( hit );
    }

    public void OnGazeExit( GazeHit hit )
    {
        if ( playBackwardsOnExit ) { DoGazeExit( hit ); }
    }

    protected abstract void DoGazeEnter( GazeHit hit );
    protected virtual void DoGazeStay( GazeHit hit ) {}
    protected abstract void DoGazeExit( GazeHit hit );
}
