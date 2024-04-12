using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // The speed at which the camera follows the target.
    public float FollowSpeed = 2f;

    // The vertical offset between the camera and the target.
    public float yOffset = 1f;

    // The target that the camera is following (usually the player).
    public Transform target;

    // Update is called once per frame.
    void Update()
    {
        // Calculate the new position for the camera using the target's x and y positions,
        // applying the vertical offset to y, and setting a fixed z position of -10.
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);

        // Update the camera's position to move towards the new position.
        // Slerp is used for spherical interpolation between two vectors, which in this case
        // results in smoother movement. FollowSpeed is multiplied by Time.deltaTime to ensure
        // that the camera's movement is frame rate independent.
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }



}
