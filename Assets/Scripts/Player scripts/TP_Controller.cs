using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour
{
    //public static CharacterController CharacterController;
    //public static TP_Controller Instance;
    TP_Motor motor;
    public bool PlayerAction = false;
    /*void Awake()
    {
        CharacterController = GetComponent<CharacterController>() as CharacterController;
        Instance = this;

        //P_Camera.UseExistingOrCreateNewMainCamera ()
        //Debug.Log(isLocalPlayer);
        //if (isLocalPlayer)
        //{
        TP_Camera.CreateNewMainCamera(netId.ToString());
        //}
    }*/

    void Awake()
    {
        motor = GetComponent<TP_Motor>();
    }
	
    void Update()
    {
        if (Camera.main == null)
        {
            return;
        } else
        { 
            GetLocomotionInput();
            HandleActionInput();
            motor.UpdateMotor();
        }
    }

    void GetLocomotionInput()
    {
        float deadZone = 0.1f;

        motor.VerticalVelocity = motor.MoveVector.y;
        motor.MoveVector = Vector3.zero;

        if (Input.GetAxis("Vertical") > deadZone || Input.GetAxis("Vertical") < -deadZone)
        {
            motor.MoveVector += new Vector3(0, 0, Input.GetAxis("Vertical"));
        }

        if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone)
        {
            motor.MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        }
    }

    void HandleActionInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerAction = true;
        }
        else
        {
            PlayerAction = false;
        }
    }

    void Jump()
    {
        motor.Jump();
    }

    /*
        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Flag" && PlayerAction == true)
            {
                other.gameObject.GetComponent<Transform>().position = gameObject.GetComponent<CharacterStats>().flagPouch.transform.position;
            }
        }
      */  
}
