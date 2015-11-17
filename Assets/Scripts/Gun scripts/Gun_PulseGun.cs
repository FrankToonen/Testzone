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
        //GetComponent<Player_Shoot>().EventShoot -= Shoot;
        GetComponent<Player_Shoot>().EventPulse += Shoot;
        rayCastLayerMask = 1 << 10;
        soundName = "pulsegun_01";
        reloadTime = 2;
        range = 20;

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
        ShootPulseGun(objectHit, point, new Vector3(f, f, f));
        primaryParticles.Play();
    }

    protected override void ShootSecondary(string objectHit, Vector3 point, float charge)
    {
        float f = force / 2;
        ShootPulseGun(objectHit, point, new Vector3(f, -f, f));
        secondaryParticles.Play();
    }

    void ShootPulseGun(string objectHit, Vector3 point, Vector3 dir)
    {
        Collider[] objectsHit = Physics.OverlapSphere(point, range);
        for (int n = 0; n < objectsHit.Length; n++)
        {
            GameObject obj = objectsHit [n].gameObject;
            if (obj.tag == "Player" || obj.tag == "PhysicsObject")
            {
                Pulse(obj, dir);
            }
        }

        StartCoroutine(ShootTimer(reloadTime));
    }

    /*void ShootPulseGun(string objectHit, Vector3 dir)
    {
        GameObject col = GameObject.Find(objectHit);
        if (col != null)
        {
            bool hasHit = false;
            
            if (col.tag == "Player" || col.tag == "PhysicsObject")
            {
                Vector3 extraAngle = new Vector3(0, 2, 0); // Schiet objecten iets omhoog
                Vector3 direction = Vector3.Scale(Vector3.Normalize(transform.position - (col.transform.position + extraAngle)), dir);
                
                if (col.tag == "Player")
                {
                    if (isServer)
                    {
                        GM_Flag flag = col.GetComponentInChildren<GM_Flag>();
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
    */

    void Pulse(GameObject obj, Vector3 dir)
    {
        Vector3 extraAngle = new Vector3(0, 2, 0); // Schiet objecten iets omhoog
        Vector3 direction = Vector3.Scale(Vector3.Normalize(transform.position - (obj.transform.position + extraAngle)), dir);
            
        if (obj.tag == "Player" && obj.name != gameObject.name)
        {
            if (isServer)
            {
                GM_Flag flag = obj.GetComponentInChildren<GM_Flag>();
                if (flag != null)
                {
                    flag.CmdChangeFlagHolder("");
                }
            }
            obj.gameObject.GetComponent<Player_Force>().AddImpact(direction, direction.magnitude);
        } else if (obj.tag == "PhysicsObject")
        {
            obj.GetComponent<Rigidbody>().AddForce(direction * 25);
        }
    }

    public override RaycastHit ShootRayCast()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 point = ray.origin + (ray.direction * range);
        hit.point = point;

        return hit;
    }
}
