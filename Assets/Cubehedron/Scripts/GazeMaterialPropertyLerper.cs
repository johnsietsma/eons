using UnityEngine;
using System.Collections;

public class GazeMaterialPropertyLerper : MonoBehaviour
{
    [SerializeField] private Material lerpMaterial;
    [SerializeField] private string propertyName;
    [SerializeField] private float toValue;
    [SerializeField] private float lerpSpeed;

    private float currentValue;
    private bool hasGaze;

    void Awake()
    {
        if( lerpMaterial==null && renderer!=null ) {
            lerpMaterial = renderer.material;
        }
    }


    void Update()
    {
        if( hasGaze ) {
            currentValue += Time.deltaTime * lerpSpeed;
            currentValue = Mathf.Min( toValue, currentValue );
            lerpMaterial.SetFloat( propertyName, currentValue );
        }
    }

    public void OnGazeEnter( GazeHit hit )
    {
        currentValue = lerpMaterial.GetFloat( propertyName );
        hasGaze = true;
    }

    public void OnGazeExit( GazeHit hit )
    {
        hasGaze = false;
    }
}
