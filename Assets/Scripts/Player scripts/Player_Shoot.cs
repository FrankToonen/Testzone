using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Shoot : NetworkBehaviour
{
    public delegate void ShootDelegate(string objectHit,Vector3 point,bool isPrimary);
    public delegate void PulseDelegate(string objectHit,Vector3 point,bool isPrimary);
    [SyncEvent]
    public event ShootDelegate
        EventShoot;

    [SyncEvent]
    public event PulseDelegate
        EventPulse;

    public Gun primaryGun;
    Gun pulseGun;

    void Start()
    {
        /*primaryGun = GetComponent<Gun_Terraformer>();
        if (primaryGun == null)
            primaryGun = GetComponent<Gun_MagnetGun>();*/

        pulseGun = GetComponent<Gun_PulseGun>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && primaryGun.CanShoot)
        {
            RaycastHit hit = primaryGun.ShootRayCast();
            if (hit.point != Vector3.zero)
            {
                CmdShoot(hit.transform.name, hit.point, Input.GetButtonDown("Fire1"));
            }
        }

        if ((Input.GetButtonDown("FirePulse1") || Input.GetButtonDown("FirePulse2")) && pulseGun.CanShoot)
        {
            RaycastHit hit = pulseGun.ShootRayCast();
            if (hit.point != Vector3.zero)
            {
                CmdPulse(hit.transform.name, hit.point, Input.GetButtonDown("FirePulse1"));
            }
        }
    }

    [Command]
    void CmdShoot(string objectHit, Vector3 point, bool isPrimary)
    {
        EventShoot(objectHit, point, isPrimary);
    }

    [Command]
    void CmdPulse(string objectHit, Vector3 point, bool isPrimary)
    {
        EventPulse(objectHit, point, isPrimary);
    }
}
