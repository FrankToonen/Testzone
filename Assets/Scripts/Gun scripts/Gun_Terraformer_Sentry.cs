using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Gun_Terraformer_Sentry : Gun_Terraformer
{
    public delegate void ShootDelegate(string objectHit,Vector3 point,float charge,bool isPrimary);
    [SyncEvent]
    public event ShootDelegate
        EventShoot;

    public float shootCharge = 1;
    public bool isPrimary = true;
    Renderer rend;

    protected override void Start()
    {
        isPlayer = false;

        base.Start();

        rend = GetComponent<Renderer>();

        reloadTime = 10;
        radius = 3;
        maxCharges = 1;
        charges = maxCharges;

        float scale = radius * shootCharge * 1.5f;
        transform.localScale = new Vector3(scale, scale, scale);

        EventShoot += Shoot;
    }

    protected override void Update()
    {
        base.Update();

        rend.enabled = charges > 0;
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || !isServer)
        {
            return;
        }

        if (charges > 0)
        {
            CmdShoot("", transform.position, shootCharge, isPrimary);
        }
    }

    [Command]
    void CmdShoot(string objectHit, Vector3 point, float charge, bool isPrimary)
    {
        EventShoot(objectHit, point, charge, isPrimary);
    }
}
