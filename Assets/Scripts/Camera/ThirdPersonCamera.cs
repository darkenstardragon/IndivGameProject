using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    private const float Y_ANGLE_MIN = -40.0f; // is really 0
    private const float Y_ANGLE_MAX = 10.0f; // is really 50
    private const float MAX_DISTANCE = 8.0f;
    private const float MIN_DISTANCE = 2.5f;
    public Transform lookAt;
    public Transform camTransform;
    public GameObject player;

    private Camera cam;

    private float distance = 5.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 4.0f;
    private float zoomSensitivity = 5.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        
    }

    private void Update()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        distance = Mathf.Clamp(distance, MIN_DISTANCE, MAX_DISTANCE);
        if (Input.GetMouseButton(0))
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY += Input.GetAxis("Mouse Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
        player.SendMessage("setCurrentX", currentX);
    }
}
