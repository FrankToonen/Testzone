using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TestPlayer : NetworkBehaviour
{
    public Terraformer gun;
    public float speed = 10f;
    Rigidbody rb;
    //Camera myCamera;

    public GameObject testPrefab;

    public delegate void ShootDelegate(Vector3 point,int dir);
    [SyncEvent]
    public event ShootDelegate
        EventShoot;

    void Start()
    {
        // myCamera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        if (isLocalPlayer)
        {
            tag = "Player";
            GetComponentInChildren<Camera>().enabled = true;
            GetComponentInChildren<AudioListener>().enabled = true;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        InputMovement();

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector3 point = gun.ShootRayCast();
            int dir = Input.GetMouseButtonDown(0) ? 1 : -1;
            if (point != Vector3.zero)
                CmdShoot(point, dir);
        }
    }

    [Command]
    public void CmdShoot(Vector3 point, int dir)
    {
        EventShoot(point, dir);
    }

    void InputMovement()
    {
        if (Input.GetKey(KeyCode.W))
            rb.MovePosition(rb.position + Vector3.forward * speed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.S))
            rb.MovePosition(rb.position - Vector3.forward * speed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.D))
            rb.MovePosition(rb.position + Vector3.right * speed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.A))
            rb.MovePosition(rb.position - Vector3.right * speed * Time.deltaTime);
    }
}
