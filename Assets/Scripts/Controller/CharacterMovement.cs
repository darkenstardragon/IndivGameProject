using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float verticalVelocity;
    public float speed = 5.0f;
    public float jumpForce = 10.0f;
    public float gravity = 14.0f;
    public float rotateSpeed = 5.0f;
    private float currentX = 0.0f;
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

        Vector3 relativeMovement = Camera.main.transform.TransformVector(move);
        relativeMovement.y = verticalVelocity;

        rotateManagement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        controller.Move(relativeMovement * Time.deltaTime);
        
    }

    private void rotateManagement(float h, float v)
    {
        if (h > 0)
        {
            if(v > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 45 + currentX, 0), Time.deltaTime * rotateSpeed);
            else if(v < 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 135 + currentX, 0), Time.deltaTime * rotateSpeed);
            else if(v == 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90 + currentX, 0), Time.deltaTime * rotateSpeed);
        }
        else if(h == 0)
        {
            if(v > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0 + currentX, 0), Time.deltaTime * rotateSpeed);
            else if(v < 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180 + currentX, 0), Time.deltaTime * rotateSpeed);
        }
        else if(h < 0)
        {
            if (v > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -45 + currentX, 0), Time.deltaTime * rotateSpeed);
            else if (v < 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -135 + currentX, 0), Time.deltaTime * rotateSpeed);
            else if (v == 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90 + currentX, 0), Time.deltaTime * rotateSpeed);
        }
    }

    public void setCurrentX(float curX)
    {
        currentX = curX;
    }
}
