// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
[ActionCategory( ActionCategory.Camera )]
[Tooltip( "Finds the point where a camera ray interescts a plane." )]
public class CameraPlaneIntersection : FsmStateAction
{
    [Tooltip( "The camera that is used to find the plane intersection." )]
    public Camera camera;

    [Tooltip( "A normalised Vector3 screen position." )]
    public FsmVector3 screenVector;

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
        camera = Camera.main;
        screenVector = null;
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
        var planeNormal = inNormal.Value;
        var plane = new Plane( planeNormal, planePoint.Value );

        //D.Log( "Intersect: {0} {1}", planeNormal, planePoint.Value );

        var rayStart = Vector3.zero;

        if ( !screenVector.IsNone ) { rayStart = screenVector.Value; }

        rayStart.x *= Screen.width;
        rayStart.y *= Screen.height;

        var cameraRay = camera.ScreenPointToRay( rayStart );

        float rayDistance = 0;
        if ( plane.Raycast( cameraRay, out rayDistance ) ) {
            storeIntersectionPoint.Value = cameraRay.GetPoint( rayDistance );
            //D.Log( "Intersection point: " + storeIntersectionPoint.Value.ToStringf() );
        }
        else {
            storeIntersectionPoint.Value = Vector3.zero;
        }
    }
}
}

