using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float verticalVelocity;
    public float speed = 5.0f;
    public float jumpForce = 10.0f;
    public float gravity = 14.0f;
    public float rotateSpeed = 10.0f;
    private float currentX = 0.0f;
    private CharacterController controller;

    private float currentRotateAngle = 0.0f;

    private bool isRushing = false;
    private const float RUSHING_TIME = 1.5f;
    private const float RUSHING_SPEED = 10.0f;
    private float currentRushingTime = 2.0f;
    private Vector3 dir;

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
            transform.SendMessage("PlayerIsOnGround", true);
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && !isRushing)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            transform.SendMessage("PlayerIsOnGround", false);
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (!isRushing)
        {
            Vector3 move = Vector3.zero;
            move.x = Input.GetAxis("Horizontal") * speed;
            move.y = verticalVelocity;
            move.z = Input.GetAxis("Vertical") * speed;

            Vector3 relativeMovement = Camera.main.transform.TransformVector(move);

            Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

            relativeMovement.y = verticalVelocity;

            rotateManagement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            controller.Move(relativeMovement * Time.deltaTime);
        }
        else
        {
            if(currentRushingTime > 0)
            {
                Vector3 move = Vector3.zero;
                move.x = dir.x * RUSHING_SPEED;
                move.y = verticalVelocity;
                move.z = dir.z * RUSHING_SPEED;
                controller.Move(move * Time.deltaTime);
                currentRushingTime -= Time.deltaTime;
            }
            else
            {
                isRushing = false;
                currentRushingTime = RUSHING_TIME;
            }
        }
        
        

    }

    private void rotateManagement(float h, float v)
    {
        if (h > 0)
        {
            if(v > 0) // W+D
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 45 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = 45;
            }
            else if(v < 0) // S+D
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 135 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = 135;
            }
            else if(v == 0) // D
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = 90;
            }
        }
        else if(h == 0)
        {
            if(v > 0) // W
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = 0;
            }
            else if(v < 0) // S
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = 180;
            }
        }
        else if(h < 0)
        {
            if (v > 0) // A+W
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -45 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = -45;
            }
            else if (v < 0) // A+S
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -135 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = -135;
            }
            else if (v == 0) // A
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90 + currentX, 0), rotateSpeed * Time.deltaTime);
                currentRotateAngle = -90;
            }
        }
    }

    public void setCurrentX(float curX)
    {
        currentX = curX;
    }

    public void Rush()
    {
        isRushing = true;
        dir = Quaternion.Euler(0, currentX + currentRotateAngle, 0) * Vector3.forward;
    }
}
