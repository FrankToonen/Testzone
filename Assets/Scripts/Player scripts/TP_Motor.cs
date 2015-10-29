using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{

    //public static TP_Motor Instance;
    CharacterController controller;

    public float MoveSpeed = 10f;
    public float Gravity = 10f;
    public Vector3 MoveVector { get; set; }

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

        //Multiply MoveVector by deltatime
        MoveVector *= Time.deltaTime;

        //Gravity pulls player down
        MoveVector -= new Vector3(0, Gravity, 0) * Time.deltaTime;


        //Move Character in world space
        controller.Move(MoveVector);
    }

    void SnapAlignCharacterWithCamera()
    {
        //if (MoveVector.x != 0 || MoveVector.z != 0)
        //{
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
        //}
    }
}
