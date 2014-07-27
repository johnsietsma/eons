using UnityEngine;
using System.Collections;

public class GazeAudio : MonoBehaviour
{
    [SerializeField] private AudioClip gazeAudio;

    void Awake()
    {
        D.Assert( audio!=null, "Gaze audio must have an audio source." );
    }

    public void OnGazeEnter( GazeHit hit )
    {
        audio.clip = gazeAudio;
        audio.Play();
    }

    public void OnGazeExit( GazeHit hit )
    {
        audio.Stop();
    }
}
