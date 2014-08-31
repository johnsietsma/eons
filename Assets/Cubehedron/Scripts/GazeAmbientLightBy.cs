using UnityEngine;
using System.Collections;

public class GazeAmbientLightBy : GazeBehaviour
{
    [SerializeField] private Color ambientColorBy;
    [SerializeField] private float time = 1;

    private Color startColor;

    protected override void DoGazeEnter( GazeHit hit )
    {
        startColor = RenderSettings.ambientLight;
        iTween.ValueTo( gameObject, iTween.Hash(
                            "name", "lerpAmbientColor",
                            "from", 0,
                            "to", 1,
                            "time", time,
                            "onupdate", "LerpAmbientColorOn"
                        )
                      );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        startColor = RenderSettings.ambientLight;
        iTween.ValueTo( gameObject, iTween.Hash(
                            "from", 0,
                            "to", 1,
                            "time", time,
                            "onupdate", "LerpAmbientColorOff"
                        )
                      );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        iTween.StopByName( gameObject, "lerpAmbientColor" );
    }

    private void LerpAmbientLightOn( float value )
    {
        RenderSettings.ambientLight = Color.Lerp( startColor, startColor + ambientColorBy, value );
    }

    private void LerpAmbientLightOff( float value )
    {
        RenderSettings.ambientLight = Color.Lerp( startColor, startColor - ambientColorBy, value );
    }
}
