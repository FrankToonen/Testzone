using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Gun : NetworkBehaviour
{
    protected Camera cam;
    protected Image reloadBar;
    protected Vector3 startScale;
    protected Vector3 targetScale;

    protected bool canShoot;
    protected float reloadTime;

    protected virtual void Start()
    {        
        cam = GetComponentInChildren<Camera>();
        GetComponent<Player_Shoot>().EventShoot += Shoot;
        canShoot = true;
    }

    /*public void UnsubscribeEvent()
    {
        GetComponent<Player_Shoot>().EventShoot -= Shoot;
    }*/

    // Temporary reload feedback
    void Update()
    {
        if (!canShoot && isLocalPlayer)
        {
            reloadBar.transform.localScale = Vector3.Lerp(reloadBar.transform.localScale, targetScale, 3 * (1 / reloadTime) * Time.deltaTime);
        }
    }

    public void Shoot(string objectHit, bool isPrimary)
    {
        if (canShoot)
        {
            if (isPrimary)
                ShootPrimary(objectHit);
            else
                ShootSecondary(objectHit);
        }
    }

    protected virtual void ShootPrimary(string objectHit)
    {

    }
    
    protected virtual void ShootSecondary(string objectHit)
    {
        
    }

    public string ShootRayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.TransformPoint(0, 0, 0.5f), cam.transform.forward);

        if (Physics.Raycast(ray, out hit))
        { 
            //Debug.DrawRay(cam.transform.TransformPoint(0, 0, 0.5f), cam.transform.forward * hit.distance, Color.blue, 10);
            return hit.transform.name;
        }

        return "";
    }

    protected IEnumerator ShootTimer(float seconds)
    {
        canShoot = false;
        reloadBar.transform.localScale = startScale;

        yield return new WaitForSeconds(seconds);

        reloadBar.transform.localScale = targetScale;

        canShoot = true;
    }
}
