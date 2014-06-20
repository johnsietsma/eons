// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
[ActionCategory( ActionCategory.Camera )]
[Tooltip( "Finds the point where a ray interescts a plane." )]
public class RaycastPlaneIntersection : FsmStateAction
{
    [Tooltip( "Start ray at game object position. \nOr use From Position parameter." )]
    public FsmOwnerDefault fromGameObject;

    [Tooltip( "Start ray at a vector3 world position. \nOr use Game Object parameter." )]
    public FsmVector3 fromPosition;

    [Tooltip( "A vector3 direction vector" )]
    public FsmVector3 direction;

    [Tooltip( "Cast the ray in world or local space. Note if no Game Object is specfied, the direction is in world space." )]
    public Space space;

    [Tooltip( "The normal of the plane." )]
    public FsmVector3 inNormal;

    [Tooltip( "A point of the plane." )]
    public FsmVector3 planePoint;

    [UIHint( UIHint.Variable )]
    [Tooltip( "Get the world position of the ray hit point and store it in a variable." )]
    public FsmVector3 storeIntersectionPoint;

    public bool everyFrame;

    public override void Reset()
    {
        fromGameObject = null;
        fromPosition = new FsmVector3 { UseVariable = true };
        direction = new FsmVector3 { UseVariable = true };
        space = Space.Self;
        inNormal = null;
        planePoint = null;
        storeIntersectionPoint = null;
    }

    public override void OnEnter()
    {
        DoPlaneIntersection();

        if ( !everyFrame ) {
            Finish();
        }
    }

    public override void OnUpdate()
    {
        DoPlaneIntersection();
    }

    void DoPlaneIntersection()
    {
        var go = Fsm.GetOwnerDefaultTarget( fromGameObject );

        var originPos = go != null ? go.transform.position : fromPosition.Value;

        var dirVector = direction.Value;
        if ( go != null && space == Space.Self ) {
            dirVector = go.transform.TransformDirection( direction.Value );
        }

        Ray ray = new Ray( originPos, dirVector );

        var planeNormal = inNormal.Value;
        var plane = new Plane( planeNormal, planePoint.Value );

        //D.Log( "Intersect: {0} {1}", planeNormal, planePoint.Value );

        float rayDistance = 0;
        if ( plane.Raycast( ray, out rayDistance ) ) {
            storeIntersectionPoint.Value = ray.GetPoint( rayDistance );
            //D.Log( "Intersection point: " + storeIntersectionPoint.Value.ToStringf() );
        }
        else {
            storeIntersectionPoint.Value = Vector3.zero;
        }
    }
}
}

