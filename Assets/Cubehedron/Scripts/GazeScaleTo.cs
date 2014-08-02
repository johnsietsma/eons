using UnityEngine;
using System.Collections;

public class GazeScaleTo : MonoBehaviour
{
    [SerializeField] private Vector3 scale;
    [SerializeField] private float speed;
    [SerializeField] private Space space;
    [SerializeField] private iTween.EaseType easeType;
    [SerializeField] private GameObject scaleObject;

    private bool hasGaze;
    private Vector3 startScale;

    void Awake()
    {
        if ( scaleObject == null ) { scaleObject = gameObject; }
        startScale = scaleObject.transform.localScale;

    }

    public void OnGazeEnter( GazeHit hit )
    {
        iTween.ScaleTo( scaleObject, iTween.Hash(
                            "scale", scale,
                            "speed", speed
                        )
                      );
    }

    public void OnGazeExit( GazeHit hit )
    {
        iTween.ScaleTo( scaleObject, iTween.Hash(
                            "scale", startScale,
                            "speed", speed
                        )
                      );
    }
}
