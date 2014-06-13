using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
[ActionCategory( ActionCategory.Input )]
[Tooltip( "Perform a raycast into the scene using screen coordinates and stores the results. Use Ray Distance to set how close the camera must be to pick the object. NOTE: Uses the MainCamera!" )]
public class CameraPick : FsmStateAction
{
    public Camera camera;

    [Tooltip( "A Vector3 screen position. Commonly stored by other actions." )]
    public FsmVector3 screenVector;
    public FsmBool normalized;
    [RequiredField]
    public FsmFloat rayDistance = 100f;
    [UIHint( UIHint.Variable )]
    public FsmBool storeDidPickObject;
    [UIHint( UIHint.Variable )]
    public FsmGameObject storeGameObject;
    [UIHint( UIHint.Variable )]
    public FsmVector3 storePoint;
    [UIHint( UIHint.Variable )]
    public FsmVector3 storeNormal;
    [UIHint( UIHint.Variable )]
    public FsmFloat storeDistance;
    [UIHint( UIHint.Layer )]
    [Tooltip("Pick only from these layers.")]
    public FsmInt[] layerMask;
    [Tooltip( "Invert the mask, so you pick from all layers except those defined above." )]
    public FsmBool invertMask;
    public bool everyFrame;

    public override void Reset()
    {
        camera = Camera.main;
        screenVector = new FsmVector3 { UseVariable = true };
        normalized = true;
        rayDistance = 100f;
        storeDidPickObject = null;
        storeGameObject = null;
        storePoint = null;
        storeNormal = null;
        storeDistance = null;
        layerMask = new FsmInt[0];
        invertMask = false;
        everyFrame = false;
    }

    public override void OnEnter()
    {
        DoCameraPick();

        if ( !everyFrame ) {
            Finish();
        }
    }

    public override void OnUpdate()
    {
        DoCameraPick();
    }

    void DoCameraPick()
    {
        if ( camera == null ) {
            LogError( "No Camera defined!" );
            Finish();
            return;
        }

        var rayStart = Vector3.zero;

        if ( !screenVector.IsNone ) { rayStart = screenVector.Value; }

        if ( normalized.Value ) {
            rayStart.x *= Screen.width;
            rayStart.y *= Screen.height;
        }

        RaycastHit hitInfo;
        var ray = camera.ScreenPointToRay( rayStart );
        Physics.Raycast( ray, out hitInfo, rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));

        var didPick = hitInfo.collider != null;
        storeDidPickObject.Value = didPick;

        if ( didPick ) {
            storeGameObject.Value = hitInfo.collider.gameObject;
            storeDistance.Value = hitInfo.distance;
            storePoint.Value = hitInfo.point;
            storeNormal.Value = hitInfo.normal;
        }
        else {
            // TODO: not sure if this is the right strategy...
            storeGameObject.Value = null;
            storeDistance = Mathf.Infinity;
            storePoint.Value = Vector3.zero;
            storeNormal.Value = Vector3.zero;
        }

    }
}
}
