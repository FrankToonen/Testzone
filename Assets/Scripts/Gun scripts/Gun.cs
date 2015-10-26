using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Gun : NetworkBehaviour
{
    protected AudioSource audioSource;
    protected string soundName;

    protected Camera cam;
    protected Image reloadBar;
    protected Vector3 startScale;
    protected Vector3 targetScale;

    protected bool canShoot;
    protected float reloadTime;

    protected virtual void Start()
    {        
        audioSource = GetComponent<AudioSource>();
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

        if (!canShoot)
            reloadBar.transform.localScale = Vector3.Lerp(reloadBar.transform.localScale, targetScale, 3 * (1 / reloadTime) * Time.deltaTime);
        else
            reloadBar.transform.localScale = targetScale;
    }

    public void Shoot(string objectHit, Vector3 point, bool isPrimary)
    {
        if (isPrimary)
            ShootPrimary(objectHit, point);
        else
            ShootSecondary(objectHit, point);

        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/snd_" + soundName);
        audioSource.PlayOneShot(audioClip);
    }

    protected virtual void ShootPrimary(string objectHit, Vector3 point)
    {

    }
    
    protected virtual void ShootSecondary(string objectHit, Vector3 point)
    {
        
    }

    public RaycastHit ShootRayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.TransformPoint(0, 0, 0.5f), cam.transform.forward);

        if (Physics.Raycast(ray, out hit))
        { 
            //Debug.DrawRay(cam.transform.TransformPoint(0, 0, 0.5f), cam.transform.forward * hit.distance, Color.blue, 10);

            StartCoroutine(ShootTimer(reloadTime));
            reloadBar.transform.localScale = startScale;

            return hit;
        }

        return new RaycastHit();
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
