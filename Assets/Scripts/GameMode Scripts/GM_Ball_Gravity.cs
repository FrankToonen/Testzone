using UnityEngine;
using System.Collections;

public class GM_Ball_Gravity : MonoBehaviour
{
    Rigidbody rBody;
    public float fallSpeed;
    float gravity = 0, gravityCap = 5000;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
	
    void Update()
    {
        gravity = Mathf.Clamp(gravity + fallSpeed * Time.deltaTime, -gravityCap, gravityCap);
        rBody.AddForce(gravity * Vector3.down * Time.deltaTime);
    }
}
