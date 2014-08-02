using UnityEngine;
using System.Collections;

public class GazeRotationFollower : MonoBehaviour {

    [SerializeField] private Gaze gazeToFollow;
    [SerializeField] private Transform followingTransform;

	void LateUpdate () {
        followingTransform.rotation = gazeToFollow.GazeTransform.rotation;
	}
}
