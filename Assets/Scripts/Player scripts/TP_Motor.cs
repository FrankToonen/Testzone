using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{

    //public static TP_Motor Instance;
    CharacterController controller;

    public float MoveSpeed = 10f;
//<<<<<<< HEAD
    public float JumpForce = 200f;
    public float Gravity = 550f;
	public float GravityCap = 800;
	public float FallSpeed = 25;

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
		MoveVector = transform.TransformDirection (MoveVector);

		//normalize MoveVector if Magnitude >1
		if (MoveVector.magnitude > 1) {
			MoveVector = Vector3.Normalize (MoveVector);
		}

		//Multiply MoveVector by MoveSpeed
		MoveVector *= MoveSpeed;

		//Reapply VerticalVelocity to moveVector.y
		MoveVector = new Vector3 (MoveVector.x, VerticalVelocity, MoveVector.z);

		if (!controller.isGrounded) {
			Gravity += FallSpeed;        
		}

        //Gravity pulls player down
        MoveVector -= new Vector3(0, Gravity, 0) * Time.deltaTime;
	
		//Checks if gravity reaches it's cap
		GravityCheck();

        //Move Character in world space
        controller.Move(MoveVector *= Time.deltaTime);

    }

	void GravityCheck(){
		if (Gravity <= -GravityCap) {
			Gravity = -GravityCap;
		} else if (Gravity >= GravityCap) {
			Gravity = GravityCap;
		}
	}

    public void Jump()
    {
        if (controller.isGrounded) {
			Gravity = -JumpForce * 5;
		} 
    }

    void SnapAlignCharacterWithCamera()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
