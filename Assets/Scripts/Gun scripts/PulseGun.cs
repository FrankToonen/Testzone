﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PulseGun : Gun
{
    //
    //Reload alleen als je iets raakt, kan mogelijk anders
    //

    [SerializeField]
    float
        force;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        GetComponent<Player_Shoot>().EventShoot -= Shoot;
        GetComponent<Player_Shoot>().EventPulse += Shoot;
        reloadTime = 2;

        //Temp reloadbar
        reloadBar = GameObject.Find("Reload Bar Pulsegun").GetComponent<Image>();
        startScale = reloadBar.transform.localScale;
        targetScale = new Vector3(0, startScale.y, 0);
        reloadBar.transform.localScale = targetScale;
        //
    }

    protected override void ShootPrimary(string objectHit)
    {
        float f = -force;
        ShootPulseGun(objectHit, new Vector3(f, f, f));
    }

    protected override void ShootSecondary(string objectHit)
    {
        float f = force / 2;
        ShootPulseGun(objectHit, new Vector3(f, -f, f));
    }

    void ShootPulseGun(string objectHit, Vector3 dir)
    {
        GameObject col = GameObject.Find(objectHit);
        if (col != null)
        {
            if (col.transform.tag == "Player" || col.transform.tag == "Cube")
            {
                Vector3 direction = Vector3.Scale(Vector3.Normalize(transform.position - (col.transform.position + new Vector3(0, 2.5f, 0))), dir);

                if (col.transform.tag == "Player")
                    col.gameObject.GetComponent<Player_Force>().AddImpact(direction, direction.magnitude);
                else
                    col.GetComponent<Rigidbody>().AddForce(direction * 25);


                StartCoroutine(ShootTimer(reloadTime));
            }
            //Play sound
        }
    }
}