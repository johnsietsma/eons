using UnityEngine;
using System.Collections;

[RequireComponent( typeof( GazePickPoint ) )]
public class PropController : MonoBehaviour
{
    [SerializeField] private GameObject gazeIndicatorObject;
    [SerializeField] private GameObject propModelObject;
    [SerializeField] private MoveToParams idleMoveToParams;
    [SerializeField] private MoveToParams selectMoveToParams;

    private Gaze currentGaze;
    private GazePickPoint gazePickPoint;

    void Awake()
    {
        gazePickPoint = GetComponent<GazePickPoint>();
        gazeIndicatorObject.SetActive( false );
        propModelObject.transform.localPosition = idleMoveToParams.position;
    }

    public Vector3 GetPickPoint()
    {
        if ( currentGaze == null ) { return Vector3.zero; }
        return gazePickPoint.GetPickPoint( currentGaze );
    }

    public void OnGazeEnter( GazeHit hit )
    {
        currentGaze = hit.gaze;
        DoSelect();
    }

    public void OnGazeExit( GazeHit hit )
    {
        currentGaze = null;
        DoIdle();
    }

    private void DoIdle()
    {
        gazeIndicatorObject.SetActive( false );
        iTween.MoveTo( propModelObject, idleMoveToParams.ToHash() );
    }

    private void DoSelect()
    {
        gazeIndicatorObject.SetActive( true );
        iTween.MoveTo( propModelObject, selectMoveToParams.ToHash() );
    }
}
