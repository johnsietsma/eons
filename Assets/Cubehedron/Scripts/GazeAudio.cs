using UnityEngine;
using System.Collections;

public class GazeAudio : GazeBehaviour
{
    [SerializeField] private AudioClip gazeAudio;

    void Awake()
    {
        D.Assert( audio!=null, "Gaze audio must have an audio source." );
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        audio.clip = gazeAudio;
        audio.Play();
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        audio.Stop();
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        audio.Stop();
    }
}
