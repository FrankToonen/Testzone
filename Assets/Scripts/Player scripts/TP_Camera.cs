using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour
{
    //public static TP_Camera Instance;
    public Transform TargetLookAt;
    public float distance = 5f;
    public float distanceMin = 3f;
    public float distanceMax = 10f;
    public float distanceSmooth = 0.0f;
    public float X_MouseSensitivity = 5f;
    public float Y_MouseSensitivity = 5f;
    public float MouseWheelSensitivity = 5f;
    public float X_smooth = 0.0f;
    public float Y_smooth = 0f;
    public float Y_MinLimit = -40f;
    public float Y_MaxLimit = 80f;
    public float OccusionDistanceStep = 0.5f;
    public int MaxOcclusionChecks = 10;
    public float DistanceResumeSmooth = 0f;

    private float _distanceSmooth = 0f;
    private float _preOccludedDistance = 0f;
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
    void Awake()
    {
        //Instance = this;
    }
	
    void Start()
    {
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        startDistance = distance;
        Reset();
    }

    void LateUpdate()
    {
        if (TargetLookAt == null)
        {
            return;
        } else
        {
            HandlePlayerInput();
            
            var count = 0;
            do
            {
                CalculateDesiredPosition();
                count++;
            } while (CheckIfOccluded(count));
            

            CheckCameraPoints(TargetLookAt.position, desiredPosition);
            UpdatePosition();
        }
    }

    void HandlePlayerInput()
    {

        float deadZone = 0.01f;
        //if (Input.GetMouseButton(1) == true) {
        mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
        mouseY += Input.GetAxis("Mouse Y") * -Y_MouseSensitivity; // min weghalen voor inverten
                                                                  //}

            mouseY = TP_Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);
        
        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity, distanceMin, distanceMax);
            _preOccludedDistance = desiredDistance;
        }
        
    }

    void CalculateDesiredPosition()
    {
        //Evaluate distance
        ResetDesiredDistance();
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref velDistance, _distanceSmooth);

        //calculate desired position
        desiredPosition = CalculatePosition(mouseY, mouseX, distance);
        
    }

    Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        return TargetLookAt.position + rotation * direction;
    }

    bool CheckIfOccluded(int count)
    {
        var  isOccluded = false;

        var nearestDistance = CheckCameraPoints(TargetLookAt.position, desiredPosition);

        if(nearestDistance != -1)
        {
            
            if(count < MaxOcclusionChecks)
            {
                isOccluded = true;
                distance -= OccusionDistanceStep; 

                if(distance <= 1f)
                {
                    distance = 1f;
                }
            }
            else
            {
                distance = nearestDistance - Camera.main.nearClipPlane;
            }
            desiredDistance = distance;
            _distanceSmooth = DistanceResumeSmooth;

        }

        return isOccluded; 
    }

    float CheckCameraPoints(Vector3 from, Vector3 to)
    {
        var NearestDistance = -1f;

        RaycastHit hitInfo;

        TP_Helper.ClipPlanePoints clipPlanePoints = TP_Helper.ClipPlaneAtNear(to);

        //Draw line in the editor to make it easier to visualize 
        Debug.DrawLine(from, to + transform.forward * -Camera.main.nearClipPlane, Color.red);
        Debug.DrawLine(from, clipPlanePoints.UpperLeft);
        Debug.DrawLine(from, clipPlanePoints.LowerLeft);
        Debug.DrawLine(from, clipPlanePoints.UpperRight);
        Debug.DrawLine(from, clipPlanePoints.UpperRight);

        Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight);
        Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight);
        Debug.DrawLine(clipPlanePoints.LowerRight, clipPlanePoints.LowerLeft); 
        Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.UpperRight);

        if(Physics.Linecast(from, clipPlanePoints.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            NearestDistance = hitInfo.distance;

        }

        if (Physics.Linecast(from, clipPlanePoints.LowerLeft, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if(hitInfo.distance < NearestDistance || NearestDistance == -1)
            {
                NearestDistance = hitInfo.distance;
            }

        }

        if (Physics.Linecast(from, clipPlanePoints.UpperRight, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < NearestDistance || NearestDistance == -1)
            {
                NearestDistance = hitInfo.distance;
            }
        }

        if (Physics.Linecast(from, clipPlanePoints.LowerRight, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < NearestDistance || NearestDistance == -1)
            {
                NearestDistance = hitInfo.distance;
            }
        }
        /*
        if (Physics.Linecast(from, to + transform.forward * -Camera.main.nearClipPlane) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < NearestDistance || NearestDistance == -1)
            {
                NearestDistance = hitInfo.distance;
            }
        }*/
        return NearestDistance;
    }

    void ResetDesiredDistance()
    {
        if(desiredDistance < _preOccludedDistance)
        {
            var pos = CalculatePosition(mouseY, mouseX, _preOccludedDistance);

            var nearestDistance = CheckCameraPoints(TargetLookAt.position, pos);

            if(nearestDistance == -1 || nearestDistance > _preOccludedDistance)
            {
                desiredDistance = _preOccludedDistance;
            }
        }

    }


    void UpdatePosition()
    {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_smooth);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_smooth);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_smooth);
        position = new Vector3(posX, posY, posZ);

        transform.position = position;
        transform.LookAt(TargetLookAt);
    }

    public void Reset()
    {
        mouseX = 0;
        mouseY = 10;
        distance = startDistance;
        desiredDistance = distance;
        _preOccludedDistance = distance;
    }


    /*public static void UseExistingOrCreateNewMainCamera()
    {
        GameObject tempCamera;
        GameObject targetLookAt;
        TP_Camera myCamera;

        if (Camera.main != null)
        {
            tempCamera = Camera.main.gameObject;
        } else
        {
            tempCamera = new GameObject("Main Camera");
            tempCamera.AddComponent<Camera>();
            tempCamera.tag = "MainCamera";
        }

        tempCamera.AddComponent<TP_Camera>();
        myCamera = tempCamera.GetComponent<TP_Camera>() as TP_Camera;

        targetLookAt = GameObject.Find("targetLookAt") as GameObject;

        if (targetLookAt == null)
        {
            targetLookAt = new GameObject("targetLookAt");
            targetLookAt.transform.position = Vector3.zero;
        }

        myCamera.TargetLookAt = targetLookAt.transform;
    }*/
}
