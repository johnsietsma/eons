using UnityEngine;
using System.Collections;

public class GazeParticles : GazeBehaviour
{
    [Tooltip( "The parent object of all the particles that will be played on gaze enter." )]
    [SerializeField] private GameObject particlesParentObject;

    private ParticleSystem[] particleSystems;


    void Awake()
    {
        if ( particlesParentObject != null ) { particleSystems = particlesParentObject.GetComponentsInChildren<ParticleSystem>(); }
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        PlayParticleSystems( particleSystems );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        StopParticleSystems( particleSystems );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        StopParticleSystems( particleSystems );
    }

    void PlayParticleSystems( ParticleSystem[] particleSystems )
    {
        if ( particleSystems == null ) { return; }
        ParticleSystem ps = null;
        for ( int i = 0; i < particleSystems.Length; i++ ) {
            ps = particleSystems[i];
            ps.Play();
        }
    }

    void StopParticleSystems( ParticleSystem[] particleSystems )
    {
        if ( particleSystems == null ) { return; }
        ParticleSystem ps = null;
        for ( int i = 0; i < particleSystems.Length; i++ ) {
            ps = particleSystems[i];
            ps.Stop();
        }
    }
}
