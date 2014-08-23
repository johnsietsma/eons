using UnityEngine;
using System.Collections;

public class GazeParticles : MonoBehaviour
{
    [Tooltip("Use all particle systems in this object and it's children.")]
    [SerializeField] private GameObject particlesParentObject;

    private ParticleSystem[] particlesSystems;

    void Awake()
    {
        D.Assert( particlesParentObject!=null, "Please set the parent object of the particle ssytem." );
        particlesSystems = GetComponentsInChildren<ParticleSystem>();
    }

    public void OnGazeEnter( GazeHit hit )
    {
        foreach( var ps in particlesSystems ) {
            ps.Play();
        }
    }

    public void OnGazeExit( GazeHit hit )
    {
        foreach( var ps in particlesSystems ) {
            ps.Stop();
        }
    }
}
