using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{

    //public static TP_Motor Instance;
    CharacterController controller;

    public float MoveSpeed = 10f;
//<<<<<<< HEAD
    public float JumpForce = 50f;
    public float Gravity = 21f;
    public float TerminalVelocity = 20f;



//=======
    //public float Gravity = 10f;
//>>>>>>> origin/master
    public Vector3 MoveVector { get; set; }
    public float VerticalVelocity { get; set; }


    void Awake()
    {
        //Instance = this;
        controller = GetComponent<CharacterController>();
    }


    public void UpdateMotor()
    {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
    }

    void ProcessMotion()
    {
        //Transform MoveVector into world space
        MoveVector = transform.TransformDirection(MoveVector);

        //normalize MoveVector if Magnitude >1
        if (MoveVector.magnitude > 1)
        {
            MoveVector = Vector3.Normalize(MoveVector);
        }

        //Multiply MoveVector by MoveSpeed
        MoveVector *= MoveSpeed;

//<<<<<<< HEAD
        //Reapply VerticalVelocity to moveVector.y
        MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);
//=======
        //Multiply MoveVector by deltatime
     //   MoveVector *= Time.deltaTime;

        //Gravity pulls player down
        MoveVector -= new Vector3(0, Gravity, 0) * Time.deltaTime;
//>>>>>>> origin/master

        //Apply gravity
         ApplyGravity();

        //Move Character in world space
        controller.Move(MoveVector *= Time.deltaTime);

    }


    void ApplyGravity()
    {
        if(MoveVector.y > -TerminalVelocity)
        {
            MoveVector = new Vector3(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime, MoveVector.z);
        }

        if(controller.isGrounded && MoveVector.y < -1)
        {
            MoveVector = new Vector3(MoveVector.x, -1, MoveVector.z);
        }
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            VerticalVelocity = JumpForce;
        }
    }

    void SnapAlignCharacterWithCamera()
    {
        //if (MoveVector.x != 0 || MoveVector.z != 0)
        //{
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        //}
    }
}
