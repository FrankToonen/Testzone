using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Shoot : NetworkBehaviour
{
    public delegate void ShootDelegate(string objectHit,bool isPrimary);
    public delegate void PulseDelegate(string objectHit,bool isPrimary);
    [SyncEvent]
    public event ShootDelegate
        EventShoot;

    [SyncEvent]
    public event PulseDelegate
        EventPulse;

    Terraformer terraformer;
    PulseGun pulseGun;

    void Start()
    {
        terraformer = GetComponent<Terraformer>();
        pulseGun = GetComponent<PulseGun>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            string objectHit = terraformer.ShootRayCast();
            if (objectHit != "")
            {
                CmdShoot(objectHit, Input.GetButtonDown("Fire1"));
            }
        }

        if (Input.GetButtonDown("FirePulse1") || Input.GetButtonDown("FirePulse2"))
        {
            string objectHit = pulseGun.ShootRayCast();
            if (objectHit != "")
            {
                CmdPulse(objectHit, Input.GetButtonDown("FirePulse1"));
            }
        }
    }

    [Command]
    void CmdShoot(string objectHit, bool isPrimary)
    {
        EventShoot(objectHit, isPrimary);
    }

    [Command]
    void CmdPulse(string objectHit, bool isPrimary)
    {
        EventPulse(objectHit, isPrimary);
    }
}
