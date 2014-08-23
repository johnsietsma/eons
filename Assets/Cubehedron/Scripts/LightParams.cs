using UnityEngine;
using System;

[Serializable]
public struct LightParams {
    public Color color;
    public float intensity;
    public float range;

    public void Apply( Light l )
    {
        l.color = color;
        l.intensity = intensity;
        l.range = range;
    }

    public static LightParams ToParams( Light l )
    {
        LightParams lp;
        lp.color = l.color;
        lp.intensity = l.intensity;
        lp.range = l.range;
        return lp;
    }

    public static LightParams Lerp( LightParams from, LightParams to, float value )
    {
        LightParams lp;
        lp.color = Color.Lerp( from.color, to.color, value );
        lp.intensity = Mathf.Lerp( from.intensity, to.intensity, value );
        lp.range = Mathf.Lerp( from.range, to.range, value );
        return lp;
    }
}
