using UnityEngine;
using System.Collections;

/**
 * Applies the rotation of the gaze to another transform.
 */
public class GazeRotationFollower : MonoBehaviour
{
    [Tooltip("The GazeInput that should be followed")]
    [SerializeField] private GazeInput gazeInputToFollow;

    [Tooltip("The transform that should match the GazeInput transform")]
    [SerializeField] private Transform followingTransform;

    void Awake()
    {
        D.Assert( gazeInputToFollow );
        D.Assert( followingTransform );
    }

    void LateUpdate ()
    {
        followingTransform.rotation = gazeInputToFollow.GazeTransform.rotation;
    }
}
