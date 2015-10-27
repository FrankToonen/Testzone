using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour
{
    //public static CharacterController CharacterController;
    //public static TP_Controller Instance;
    TP_Motor motor;

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

            motor.UpdateMotor();
        }
    }

    void GetLocomotionInput()
    {
        float deadZone = 0.1f;

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
}
