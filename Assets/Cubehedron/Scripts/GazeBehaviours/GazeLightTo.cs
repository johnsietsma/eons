using UnityEngine;
using System.Collections;

public class GazeLightTo : GazeBehaviour
{
    [SerializeField] private Light gazeLight;
    [SerializeField] private LightParams lightToParams;
    [SerializeField] private float time = 1;

    private ValueLerper<LightParams> lightLerper;
    private LightParams startParams;

    void Awake()
    {
        if ( gazeLight == null ) { gazeLight = GetComponent<Light>(); }
        D.Assert( gazeLight != null, "Please assign the gazeLight." );
        lightLerper = new ValueLerper<LightParams>( gameObject, LightParams.Lerp );
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        startParams = LightParams.ToParams( gazeLight );
        lightLerper.Lerp( startParams, lightToParams, lp=>lp.Apply(gazeLight), time );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        var currLightParams = LightParams.ToParams( gazeLight );
        lightLerper.Lerp( currLightParams, startParams, lp=>lp.Apply(gazeLight), time );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        lightLerper.Stop();
    }
}
