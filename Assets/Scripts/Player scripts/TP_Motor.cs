using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{

    //public static TP_Motor Instance;
    CharacterController controller;

    public float moveSpeed = 10f;
//<<<<<<< HEAD
    public float jumpForce = 200f;
    public float gravity = 550f;
    public float gravityCap = 800, gravityMax;
    public float fallSpeed = 25;

//=======
    //public float Gravity = 10f;
//>>>>>>> origin/master
    public Vector3 moveVector { get; set; }
    public float verticalVelocity { get; set; }


    void Awake()
    {
        //Instance = this;
        controller = GetComponent<CharacterController>();
        gravityMax = gravityCap;
    }

    public void UpdateMotor()
    {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
    }

    void ProcessMotion()
    {
        //Transform MoveVector into world space
        moveVector = transform.TransformDirection(moveVector);

        //normalize MoveVector if Magnitude >1
        if (moveVector.magnitude > 1)
        {
            moveVector = Vector3.Normalize(moveVector);
        }

        //Multiply MoveVector by MoveSpeed
        moveVector *= moveSpeed;

        //Reapply VerticalVelocity to moveVector.y
        moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);

        gravityCap = controller.isGrounded ? 50 : gravityMax;
        gravity = Mathf.Clamp(gravity + fallSpeed * Time.deltaTime, -gravityCap, gravityCap);

        //Gravity pulls player down
        moveVector -= new Vector3(0, gravity, 0);
	
        //Checks if gravity reaches it's cap
        //GravityCheck();

        //Move Character in world space
        controller.Move(moveVector *= Time.deltaTime);
    }

    /*void GravityCheck()
    {
        if (gravity <= -gravityCap)
        {
            gravity = -gravityCap;
        } else if (gravity >= gravityCap)
        {
            gravity = gravityCap;
        }
    }*/

    public void Jump()
    {
        if (controller.isGrounded)
        {
            gravity = -jumpForce * 5;
        } 
    }

    void SnapAlignCharacterWithCamera()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
