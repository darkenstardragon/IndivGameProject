using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float normalSpeed = 5.0f;
    private const float slowedSpeed = 2.0f;

    private float verticalVelocity;
    public float speed;
    public float jumpForce = 6.0f;
    public float gravity = 30.0f;
    public float rotateSpeed = 300.0f;
    private float currentX = 0.0f;
    private CharacterController controller;

    private float currentRotateAngle = 0.0f;

    private Vector3 dir;

    private bool isRushing = false;
    private const float RUSHING_TIME = 1.5f;
    private const float RUSHING_SPEED = 10.0f;
    private float currentRushingTime;

    private bool isDodging = false;
    private const float DODGING_TIME = 0.3f;
    private const float DODGING_SPEED = 8.0f;
    private float currentDodgingTime;

    private bool isAfterCastMoving = false;
    private const float AFTERCAST_MOVEMENT_SPEED = 10.0f;
    private float customAfterCastMovementSpeed = 0.0f;
    private float afterCastMovingTime = 0.0f;

    private Animator anim;

    private float currentTimeSinceLastGrounded;
    private const float LAST_GROUNDED = 0.3f;

    private bool isCharging = false;

    private bool instantRotate = true;

    private bool isMovementDisabled = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        currentDodgingTime = DODGING_TIME;
        currentRushingTime = RUSHING_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (controller.isGrounded)
        {
            currentTimeSinceLastGrounded = LAST_GROUNDED;
            transform.SendMessage("PlayerIsOnGround", true);
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && !isRushing && !isDodging && !isCharging && !isMovementDisabled)
            {
                verticalVelocity = jumpForce;
                anim.SetBool("isJumping", true);
            }
            
            anim.SetBool("isJumping", false);
            //print("isGrounded");
        }
        else
        {
            currentTimeSinceLastGrounded -= Time.deltaTime;
            if(currentTimeSinceLastGrounded < 0)
            {
                //print("isNotGrounded");
                anim.SetBool("isJumping", true);
                
            }
            transform.SendMessage("PlayerIsOnGround", false);
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (!isRushing && !isDodging && !isMovementDisabled && !isAfterCastMoving)
        {
            speed = isCharging ? slowedSpeed : normalSpeed;

            Vector3 move = Vector3.zero;
            move.x = Input.GetAxis("Horizontal") * speed;
            //move.y = verticalVelocity;
            move.z = Input.GetAxis("Vertical") * speed;

            Vector3 relativeMovement = Camera.main.transform.TransformVector(move);

            Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

            relativeMovement.y = verticalVelocity;

            RotateManagement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            controller.Move(relativeMovement * Time.deltaTime);
            //anim.Play("walk");
            float spd = Mathf.Sqrt(move.x * move.x + move.z * move.z);
            anim.SetFloat("InputY", -spd);

            if (move.x != 0 || move.z != 0)
            {
                transform.SendMessage("SetMoving", true);
                //print("setmove = true");
            }
            else
            {
                transform.SendMessage("SetMoving", false);
                //print("setmove = false : " + move.x + " " + move.z);
            }
        }
        else if(isRushing)
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
        else if (isDodging)
        {
            //print("3");
            if(currentDodgingTime > 0)
            {
                //print("4");
                Vector3 move = Vector3.zero;
                move.x = dir.x * DODGING_SPEED;
                move.y = verticalVelocity;
                move.z = dir.z * DODGING_SPEED;
                controller.Move(move * Time.deltaTime);
                currentDodgingTime -= Time.deltaTime;
            }
            else
            {
                //print("5");
                isDodging = false;
                currentDodgingTime = DODGING_TIME;
                anim.SetBool("isRolling", false);
            }
        }
        else if (isAfterCastMoving)
        {
            if(afterCastMovingTime > 0)
            {
                //int("innn");
                Vector3 move = Vector3.zero;

                move.x = customAfterCastMovementSpeed > 0 ? dir.x * customAfterCastMovementSpeed : dir.x * AFTERCAST_MOVEMENT_SPEED;
                move.y = verticalVelocity;
                move.z = customAfterCastMovementSpeed > 0 ? dir.z * customAfterCastMovementSpeed : dir.z * AFTERCAST_MOVEMENT_SPEED;
                controller.Move(move * Time.deltaTime);
                afterCastMovingTime -= Time.deltaTime;
            }
            else
            {
                isAfterCastMoving = false;
                afterCastMovingTime = 0.0f;
                customAfterCastMovementSpeed = 0.0f;
            }
        }
        
        

    }

    private void RotateManagement(float h, float v)
    {
        float multiplyFactor = Time.deltaTime;
        if (instantRotate) multiplyFactor = 1;

        if (h > 0)
        {
            if(v > 0) // W+D
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 45 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = 45;
            }
            else if(v < 0) // S+D
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 135 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = 135;
            }
            else if(v == 0) // D
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = 90;
            }
        }
        else if(h == 0)
        {
            if(v > 0) // W
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = 0;
            }
            else if(v < 0) // S
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = 180;
            }
        }
        else if(h < 0)
        {
            if (v > 0) // A+W
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -45 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = -45;
            }
            else if (v < 0) // A+S
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -135 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = -135;
            }
            else if (v == 0) // A
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90 + currentX, 0), rotateSpeed * multiplyFactor);
                currentRotateAngle = -90;
            }
        }
    }

    public void SetCurrentX(float curX)
    {
        currentX = curX;
    }

    public void Rush()
    {
        isRushing = true;
        dir = Quaternion.Euler(0, currentX + currentRotateAngle, 0) * Vector3.forward;
    }

    public void Dodge()
    {
        anim.SetBool("isRolling", true);
        isDodging = true;
        dir = Quaternion.Euler(0, currentX + currentRotateAngle, 0) * Vector3.forward;
    }

    public void AfterCastMoving(float time)
    {
        isAfterCastMoving = true;
        afterCastMovingTime = time;
        dir = Quaternion.Euler(0, currentX + currentRotateAngle, 0) * Vector3.forward;
    }

    public void AfterCastMoving(float[] timeAndSpeed)
    {
        isAfterCastMoving = true;
        afterCastMovingTime = timeAndSpeed[0];
        customAfterCastMovementSpeed = timeAndSpeed[1];
        dir = Quaternion.Euler(0, currentX + currentRotateAngle, 0) * Vector3.forward;
    }

    public void SetCharging(bool b)
    {
        isCharging = b;
    }

    public void SetMovementDisable(bool b)
    {
        isMovementDisabled = b;
        anim.SetFloat("InputY", 0);
    }
    
}
