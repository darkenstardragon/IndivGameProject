using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour
{
    
    public float jumpForce = 10.0f;
    private CharacterController controller;
    public float speed = 5.0f;
    private Vector3 moveDirection;
    public float gravityScale = 1.0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
        }
        else
        {
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);
        }

        controller.Move(moveDirection * Time.deltaTime);
    }
}
