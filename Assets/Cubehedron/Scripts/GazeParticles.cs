using UnityEngine;
using System.Collections;

public class GazeParticles : GazeBehaviour
{
    [Tooltip("Use all particle systems in this object and it's children.")]
    [SerializeField] private GameObject particlesParentObject;

    private ParticleSystem[] particlesSystems;


    void Awake()
    {
        D.Assert( particlesParentObject!=null, "Please set the parent object of the particle ssytem." );
        particlesSystems = particlesParentObject.GetComponentsInChildren<ParticleSystem>();
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        foreach( var ps in particlesSystems ) {
            ps.Play();
        }
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        D.Log( "Stop praticles: {0}", name );
        foreach( var ps in particlesSystems ) {
            ps.Stop();
        }
    }
}
