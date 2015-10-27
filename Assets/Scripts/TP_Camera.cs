using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour {

    public static TP_Camera Instance;
    public Transform TargetLookAt;
    public float distance = 5f;
    public float distanceMin = 3f;
    public float distanceMax = 10f;
    public float distanceSmooth = 0.05f;
    public float X_MouseSensitivity = 30f;
    public float Y_MouseSensitivity = 17f;
    public float MouseWheelSensitivity = 5f;
    public float X_smooth = 0.05f;
    public float Y_smooth = 0.1f;
    public float Y_MinLimit = -40f;
    public float Y_MaxLimit = 80f;



    private float mouseX = 0f;
    private float mouseY = 0f;
    private float velX = 0f;
    private float velY = 0f;
    private float velZ = 0f;
    private float velDistance = 0f;
    private float startDistance = 0f;
    private float desiredDistance = 0f;
    private Vector3 position = Vector3.zero;
    private Vector3 desiredPosition = Vector3.zero;

	void Awake () {
        Instance = this;
	}
	
    void Start ()
    {
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        startDistance = distance;
        Reset();
    }

	void LateUpdate () {
        if (TargetLookAt == null)
        {
            return;
        }else
        {
            HandlePlayerInput();
            CalculateDesiredPosition();
            UpdatePosition();
        }
	}

    void HandlePlayerInput() {

        float deadZone = 0.01f;
        //if (Input.GetMouseButton(1) == true) {
            mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
            mouseY += Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
        //}

        mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

        if(Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity, distanceMin, distanceMax);
        }

    }


    void CalculateDesiredPosition() {
        //Evaluate distance
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref velDistance, distanceSmooth);

        //calculate desired position
        desiredPosition = CalculatePosition(mouseY, mouseX, distance);
        
    }

    Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        return TargetLookAt.position + rotation * direction;
    }

    void UpdatePosition() {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_smooth);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_smooth);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_smooth);
        position = new Vector3(posX, posY, posZ);

        transform.position = position;
        transform.LookAt(TargetLookAt);
    }

    public void Reset(){
        mouseX = 0;
        mouseY = 10;
        distance = startDistance;
        desiredDistance = distance;
    }

	void OnGUI(){
		GUI.Label(new Rect((Screen.width/2)-8,(Screen.height/3)-3,16,16),"+");
	}

    public static void UseExistingOrCreateNewMainCamera()
    {
        GameObject tempCamera;
        GameObject targetLookAt;
        TP_Camera myCamera;

        if (Camera.main != null){
            tempCamera = Camera.main.gameObject;
        }
        else {
            tempCamera = new GameObject("Main Camera");
            tempCamera.AddComponent<Camera>();
            tempCamera.tag = "Main camera";
        }

        tempCamera.AddComponent<TP_Camera>();
        myCamera = tempCamera.GetComponent<TP_Camera>() as TP_Camera;

        targetLookAt = GameObject.Find("targetLookAt") as GameObject;

        if (targetLookAt == null) {
            targetLookAt = new GameObject("targetLookAt");
            targetLookAt.transform.position = Vector3.zero;
        }

        myCamera.TargetLookAt = targetLookAt.transform;
    }

}
