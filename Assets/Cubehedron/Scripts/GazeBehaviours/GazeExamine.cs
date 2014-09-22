using UnityEngine;
using System.Collections;

public class GazeExamine : GazeBehaviour
{
    [SerializeField] private float examineRadius;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 examineOffset;

    private Vector3 startPos;
    private GazeInput gazeInput;

    private BoxCollider boxCollider;
    private Vector3 boxColliderStartSize;

    void Awake()
    {
        startPos = transform.position;

        if( collider && collider is BoxCollider ) {
          boxCollider = collider as BoxCollider;
          boxColliderStartSize = boxCollider.size;
        }
    }

    protected override void DoGazeEnter( GazeHit hit )
    {
        gazeInput = hit.gazeInput;
        var pos = gazeInput.GazeTransform.forward * examineRadius;

        if( boxCollider!=null ) {
          boxCollider.size = boxColliderStartSize * 2;
        }

        iTween.MoveTo( gameObject, iTween.Hash(
                           "name", "gazeExamine",
                           "position", gazeInput.GazeTransform.position + pos + examineOffset,
                           "speed", speed,
                           "space", Space.World
                       ) );
    }

    protected override void DoGazeExit( GazeHit hit )
    {
        iTween.MoveTo( gameObject, iTween.Hash(
                           "position", startPos,
                           "speed", speed,
                           "space", Space.World
                       ) );

        gazeInput = null;

        if( boxCollider!=null ) {
          boxCollider.size = boxColliderStartSize;
        }
    }

    protected override void DoGazeStop( GazeHit hit )
    {
        iTween.StopByName( gameObject, "gazeExamine" );

        if( boxCollider!=null ) {
          boxCollider.size = boxColliderStartSize;
        }
    }

}
