using UnityEngine;
using System.Collections;

public class GazeLightTo : GazeBehaviour
{
    [SerializeField] private Light gazeLight;
    [SerializeField] private LightParams lightToParams;
    [SerializeField] private float time = 1;

    private LightParams startParams;

    void Awake()
    {
        if ( gazeLight == null ) { gazeLight = GetComponent<Light>(); }
        D.Assert( gazeLight != null, "Please assign the gazeLight." );
        startParams = LightParams.ToParams( gazeLight );

    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        iTween.ValueTo( gameObject, iTween.Hash(
                            "from", 0,
                            "to", 1,
                            "time", time,
                            "onupdate", "LerpLightParams"
                        )
                      );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.ValueTo( gameObject, iTween.Hash(
                            "from", 1,
                            "to", 0,
                            "time", time,
                            "onupdate", "LerpLightParams"
                        )
                      );
    }

    private void LerpLightParams( float value )
    {
        LightParams lp = LightParams.Lerp( startParams, lightToParams, value );
        lp.Apply( gazeLight );
    }
}
