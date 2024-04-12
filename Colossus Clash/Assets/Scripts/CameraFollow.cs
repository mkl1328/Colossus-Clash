using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float followSpeed = 2f;
    public float yOffset = 1f;
    public float buffer = 2f; // Buffer zone before camera starts moving faster vertically, both top and bottom

    private PlayerController playerController;
    private float cameraHeightHalf;

    void Start()
    {
        playerController = target.GetComponent<PlayerController>();
        cameraHeightHalf = Camera.main.orthographicSize; // Get half of the camera's height
    }

    void Update()
    {
        // Calculate the camera's top and bottom edge positions in world coordinates
        float cameraTopEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float cameraBottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        // Check distances to top and bottom buffer zones
        float distanceToTop = (target.position.y + yOffset) - cameraTopEdge;
        float distanceToBottom = (target.position.y + yOffset) - cameraBottomEdge;

        // Adjust vertical follow speed based on buffer zones or player dashing
        float currentVerticalFollowSpeed = followSpeed;
        if (distanceToTop > -buffer || distanceToBottom < buffer || playerController.isDashing)
        {
            currentVerticalFollowSpeed = followSpeed * 3; // Increase follow speed when near buffer zones or dashing
        }

        // Calculate the new position with offset, maintaining a fixed z-axis
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);

        // Adjust y component dynamically based on calculated follow speed
        Vector3 verticalPos = new Vector3(transform.position.x, newPos.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, verticalPos, currentVerticalFollowSpeed * Time.deltaTime);

        // Adjust x and z components normally
        Vector3 horizontalPos = new Vector3(newPos.x, transform.position.y, newPos.z);
        transform.position = Vector3.Lerp(transform.position, horizontalPos, followSpeed * Time.deltaTime);
    }


}
