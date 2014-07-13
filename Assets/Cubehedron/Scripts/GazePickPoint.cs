using UnityEngine;
using System.Collections;

public class GazePickPoint : MonoBehaviour
{
    private Bounds propBounds;

    void Awake()
    {
        propBounds = new Bounds( transform.position, Vector3.zero );
        foreach( var c in GetComponents<Collider>() ) {
            propBounds.Encapsulate( c.bounds );
        }
    }

    /**
     * Find a point in the center of the prop that is under the gaze ray.
     */
    public Vector3 GetPickPoint( Gaze gaze )
    {
        // The horizontal plane that goes through the center of the prop.
        var propPlane = new Plane( transform.up, propBounds.center );

        var gazeRay = new Ray( gaze.GazeTransform.position, gaze.GazeTransform.forward );

        float rayDistance;
        Vector3 pickPoint = Vector3.zero;
        if ( propPlane.Raycast( gazeRay, out rayDistance ) ) {
            pickPoint = gazeRay.GetPoint( rayDistance );
        }

        return pickPoint;
    }

}
