using System;
using UnityEngine;

public class FloatLerper : ValueLerper<float>
{
    public FloatLerper( GameObject gameObject ) : base( gameObject, Mathf.Lerp ) {}
}

public class ColorLerper : ValueLerper<Color>
{
    public ColorLerper( GameObject gameObject ) : base( gameObject, Color.Lerp ) {}
}

public class ValueLerper<T>
{
    public delegate T LerpDelegate( T v1, T v2, float t );

    private Func<T> valueGetter;
    private Action<T> valueSetter;
    private LerpDelegate lerper;

    private GameObject gameObject;
    private T fromValue;
    private T toValue;

    public ValueLerper( GameObject gameObject_, LerpDelegate lerper_ )
    {
        gameObject = gameObject_;
        lerper = lerper_;
    }

    public void Lerp( T startValue_, T toValue_, Action<T> valueSetter_, float time )
    {
        fromValue = startValue_;
        toValue = toValue_;
        valueSetter = valueSetter_;

        iTween.ValueTo( gameObject, iTween.Hash(
                            "name", "lerpValue",
                            "from", 0,
                            "to", 1,
                            "time", time,
                            "onupdate", ( Action<object> )( v => UpdateValueLerp( ( float )v ) )
                        )
                      );
    }

    public void Stop()
    {
        iTween.StopByName( gameObject, "lerpValue" );
    }


    private void UpdateValueLerp( float t )
    {
        D.Log( "UpdateValueLerp" );
        T newValue = lerper( fromValue, toValue, t );
        valueSetter( newValue );
    }

}
