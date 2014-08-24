using UnityEngine;
using System.Collections;

public class GazeAmbientLightTo : GazeBehaviour
{
    [SerializeField] private Color ambientColorTo;
    [SerializeField] private float time = 1;

    private Color startColor;

    void Awake()
    {
        startColor = RenderSettings.ambientLight;
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
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
        RenderSettings.ambientLight = Color.Lerp( startColor, ambientColorTo, value );
    }
}
