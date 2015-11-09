using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun_PulseGun : Gun
{
    [SerializeField]
    float
        force;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        GetComponent<Player_Shoot>().EventShoot -= Shoot;
        GetComponent<Player_Shoot>().EventPulse += Shoot;
        rayCastLayerMask = 1 << 10;
        soundName = "pulsegun_01";
        reloadTime = 2;

        //Temp reloadbar
        if (isLocalPlayer)
        {
            reloadBar = GameObject.Find("Reload Bar Pulsegun").GetComponent<Image>();
            startScale = reloadBar.transform.localScale;
            targetScale = new Vector3(0, startScale.y, 0);
            reloadBar.transform.localScale = targetScale;
        }
        //
    }

    protected override void ShootPrimary(string objectHit, Vector3 point, float charge)
    {
        float f = -force;
        ShootPulseGun(objectHit, new Vector3(f, f, f));
    }

    protected override void ShootSecondary(string objectHit, Vector3 point, float charge)
    {
        float f = force / 2;
        ShootPulseGun(objectHit, new Vector3(f, -f, f));
    }

    void ShootPulseGun(string objectHit, Vector3 dir)
    {
        GameObject col = GameObject.Find(objectHit);
        if (col != null)
        {
            bool hasHit = false;

            if (col.transform.tag == "Player" || col.transform.tag == "PhysicsObject")
            {
                Vector3 extraAngle = new Vector3(0, 2, 0); // Schiet objecten iets omhoog
                Vector3 direction = Vector3.Scale(Vector3.Normalize(transform.position - (col.transform.position + extraAngle)), dir);

                if (col.transform.tag == "Player")
                {
                    if (isServer)
                    {
                        CTF_Flag flag = col.GetComponentInChildren<CTF_Flag>();
                        if (flag != null)
                        {
                            flag.CmdChangeFlagHolder("");
                        }
                    }
                    col.gameObject.GetComponent<Player_Force>().AddImpact(direction, direction.magnitude);
                    hasHit = true;
                } else
                {
                    col.GetComponent<Rigidbody>().AddForce(direction * 25);
                    hasHit = true;
                }
            }

            if (hasHit)
            {
                StartCoroutine(ShootTimer(reloadTime));
            }
        }
    }
}
