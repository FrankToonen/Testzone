﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Shoot : NetworkBehaviour
{
    public delegate void ShootDelegate(string objectHit,Vector3 point,float charge,bool isPrimary);
    public delegate void PulseDelegate(string objectHit,Vector3 point,float charge,bool isPrimary);
    [SyncEvent]
    public event ShootDelegate
        EventShoot;

    [SyncEvent]
    public event PulseDelegate
        EventPulse;

    float prevLTValue, prevRTValue;
    Gun primaryGun, pulseGun;

    void Start()
    {
        /*primaryGun = GetComponent<Gun_Terraformer>();
        if (primaryGun == null)
            primaryGun = GetComponent<Gun_MagnetGun>();*/

        primaryGun = GetComponent<Gun_Terraformer>();
        pulseGun = GetComponent<Gun_PulseGun>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        float currentLTValue = Input.GetAxis("Fire2");
        //bool LTPressed = prevLTValue < currentLTValue;
        bool LTReleased = prevLTValue > currentLTValue;

        float currentRTValue = Input.GetAxis("Fire1");
        //bool RTPressed = prevRTValue < currentRTValue;
        bool RTReleased = prevRTValue > currentRTValue;

        if (primaryGun.CanShoot)
        {
            if (Input.GetAxis("Fire1") == 1 || Input.GetAxis("Fire2") == 1)
            {
                primaryGun.ChargeGun(Time.deltaTime);
            } else if (LTReleased || RTReleased)
            {
                RaycastHit hit = primaryGun.ShootRayCast();
                if (hit.point != Vector3.zero)
                {
                    CmdShoot(hit.transform.name, hit.point, primaryGun.Charge, RTReleased);
                }
            }
        }
       
        /*if ((//Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") ||
             LTPressed || RTPressed) && primaryGun.CanShoot)
        {
            //Debug.Log(LTPressed + " | " + RTPressed);
            RaycastHit hit = primaryGun.ShootRayCast();
            if (hit.point != Vector3.zero)
            {
                CmdShoot(hit.transform.name, hit.point, //Input.GetButtonDown("Fire1") ||
                RTPressed);
            }
        }*/



        if (pulseGun.CanShoot)
        {
            if (Input.GetButton("FirePulse1") || Input.GetButton("FirePulse2"))
            {
                pulseGun.ChargeGun(Time.deltaTime);
            } else if (Input.GetButtonUp("FirePulse1") || Input.GetButtonUp("FirePulse2"))
            {
                /*RaycastHit hit = pulseGun.ShootRayCast();
                if (hit.point != Vector3.zero)
                {
                    CmdPulse(hit.transform.name, hit.point, pulseGun.Charge, Input.GetButtonUp("FirePulse1"));
                }*/

                RaycastHit hit = pulseGun.ShootRayCast();

                CmdPulse("", hit.point, 0, Input.GetButtonUp("FirePulse1"));
            }
        }

        /*if ((Input.GetButtonDown("FirePulse1") || Input.GetButtonDown("FirePulse2")) && pulseGun.CanShoot)
        {
            RaycastHit hit = pulseGun.ShootRayCast();
            if (hit.point != Vector3.zero)
            {
                CmdPulse(hit.transform.name, hit.point, 0, Input.GetButtonDown("FirePulse1"));
            }
        }*/

        prevLTValue = currentLTValue;
        prevRTValue = currentRTValue;
    }

    [Command]
    void CmdShoot(string objectHit, Vector3 point, float charge, bool isPrimary)
    {
        EventShoot(objectHit, point, charge, isPrimary);
    }

    [Command]
    void CmdPulse(string objectHit, Vector3 point, float charge, bool isPrimary)
    {
        EventPulse(objectHit, point, charge, isPrimary);
    }
}
