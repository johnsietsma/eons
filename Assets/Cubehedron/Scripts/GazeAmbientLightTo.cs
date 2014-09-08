using UnityEngine;
using System.Collections;

public class GazeAmbientLightTo : GazeBehaviour
{
    [SerializeField] private Color ambientColorTo;
    [SerializeField] private float time = 1;

    private ColorLerper colorLerper;
    private Color fromColor;

    void Awake()
    {
        colorLerper = new ColorLerper( gameObject );
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        fromColor = RenderSettings.ambientLight;
        colorLerper.Lerp( fromColor, ambientColorTo, v => RenderSettings.ambientLight = v, time );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        var colorDiff = RenderSettings.ambientLight - fromColor;
        colorLerper.Lerp( RenderSettings.ambientLight, fromColor - colorDiff, v => RenderSettings.ambientLight = v, time );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        colorLerper.Stop();
    }
}
