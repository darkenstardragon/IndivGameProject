using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float verticalVelocity;
    public float speed = 5.0f;
    public float jumpForce = 10.0f;
    public float gravity = 14.0f;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        Vector3 move = Vector3.zero;
        move.x = Input.GetAxis("Horizontal") * speed;
        move.y = verticalVelocity;
        move.z = Input.GetAxis("Vertical") * speed;
        controller.Move(move * Time.deltaTime);

    }
}
