using UnityEngine;
using System.Collections;

public class GM_Ball_Gravity : MonoBehaviour
{
    Rigidbody rBody;
    public float fallSpeed;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
	
    void Update()
    {
        rBody.AddForce(fallSpeed * Vector3.down);
    }
}
