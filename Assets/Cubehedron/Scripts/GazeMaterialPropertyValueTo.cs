using UnityEngine;
using System.Collections;

public class GazeMaterialPropertyValueTo : GazeBehaviour
{
    [SerializeField] private Material propertyMaterial;
    [SerializeField] private string propertyName;
    [SerializeField] private float toValue;
    [SerializeField] private float time = 1;

    private float fromValue;
    private FloatLerper propertyValueLerper;

    void Awake()
    {
        propertyValueLerper = new FloatLerper( gameObject );
        if ( propertyMaterial == null && renderer != null ) {
            propertyMaterial = renderer.material;
        }
        D.Assert( propertyMaterial.HasProperty( propertyName ), "Material {0} doesn't have property with name {1},", propertyMaterial.name, propertyName );
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        fromValue = propertyMaterial.GetFloat( propertyName );
        D.Log( "PropValueEnter: from:{0} to:{1}", fromValue, toValue );
        propertyValueLerper.Lerp( fromValue, toValue, SetPropertyValue , time );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        var currentValue = propertyMaterial.GetFloat( propertyName );
        var valueDelta = currentValue - fromValue;
        D.Log( "PropValueExit: from:{0} to:{1}", currentValue, ( currentValue - valueDelta ) );
        propertyValueLerper.Lerp( currentValue, currentValue - valueDelta, SetPropertyValue, time );
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        D.Log( "PropvalueStop" );
        propertyValueLerper.Stop();
    }

    private void SetPropertyValue( float v )
    {
        propertyMaterial.SetFloat( propertyName, v );
    }
}
