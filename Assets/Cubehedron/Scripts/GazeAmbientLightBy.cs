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
                            "from", 0,
                            "to", 1,
                            "time", time,
                            "onupdate", "LerpAmbientLight"
                        )
                      );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.ValueTo( gameObject, iTween.Hash(
                            "from", 1,
                            "to", 0,
                            "time", time,
                            "onupdate", "LerpAmbientLight"
                        )
                      );
    }

    private void LerpAmbientLight( float value )
    {
        RenderSettings.ambientLight = Color.Lerp( startColor, startColor + ambientColorBy, value );
    }
}
