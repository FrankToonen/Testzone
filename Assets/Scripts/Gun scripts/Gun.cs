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

    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Werkt niet als client, wel als host
        if (!canShoot)
            reloadBar.transform.localScale = Vector3.Lerp(reloadBar.transform.localScale, targetScale, 3 * (1 / reloadTime) * Time.deltaTime);
        else
            reloadBar.transform.localScale = targetScale;
    }

    public void Shoot(string objectHit, bool isPrimary)
    {
        if (isPrimary)
            ShootPrimary(objectHit);
        else
            ShootSecondary(objectHit);
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

            StartCoroutine(ShootTimer(reloadTime));
            reloadBar.transform.localScale = startScale;

            return hit.transform.name;
        }

        return "";
    }

    protected IEnumerator ShootTimer(float seconds)
    {
        canShoot = false;
        yield return new WaitForSeconds(seconds);
        canShoot = true;
    }

    public bool CanShoot
    {
        get { return canShoot; }
    }
}
