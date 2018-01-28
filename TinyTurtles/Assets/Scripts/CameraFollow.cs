using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;  //tracks the player the camera needs to follow
    public Transform Loc;  //tracks the player the camera needs to follow

    /// <summary>
    /// Step 1: Camera follows the player.
    /// </summary>
    void LateUpdate () {
        Vector3 desiredPosition = Loc.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.05f);
        transform.position = smoothedPosition;
        transform.LookAt(target);
        //Note: Camera movement is still jerky and needs improvement.
    }
}
