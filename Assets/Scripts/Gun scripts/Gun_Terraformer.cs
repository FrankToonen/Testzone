using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Networking;
using System.Collections;

public class Gun_Terraformer : Gun
{
    [SerializeField]
    float
        radius;
    public float chargedRadius;

    protected override void Start()
    {
        base.Start();
        soundName = /*"terraformer_01"*/ "pulsegun_02";
        reloadTime = 2;
        radius = 3;
        maxCharges = 3;
        charges = maxCharges;

        //GetComponent<Player_Shoot>().EventShoot -= Shoot;
        GetComponent<Player_Shoot>().EventShoot += Shoot;

        //Temp reloadbar
        if (isLocalPlayer)
        {
            reloadBar = GameObject.Find("Reload Bar Terraformer").GetComponent<Image>();
            startScale = reloadBar.transform.localScale;
            targetScale = new Vector3(0, startScale.y, 0);
            reloadBar.transform.localScale = targetScale;
        }
        //
    }

    protected override void ShootPrimary(string objectHit, Vector3 point, float charge)
    {
        base.ShootPrimary(objectHit, point, charge);

        ShootTerraformer(objectHit, point, charge, 1);
    }

    protected override void ShootSecondary(string objectHit, Vector3 point, float charge)
    {
        base.ShootSecondary(objectHit, point, charge);

        ShootTerraformer(objectHit, point, charge, -1);
    }

    void ShootTerraformer(string objectHit, Vector3 point, float charge, int dir)
    {
        bool hasHit = false;
        float chargedRadius = radius * Mathf.Clamp(charge, 1, maxChargeTime);

        Collider[] objectsHit = Physics.OverlapSphere(point, chargedRadius);
        for (int n = 0; n < objectsHit.Length; n++)
        {
            HexChunk hexChunk = objectsHit [n].GetComponent<HexChunk>();
            if (hexChunk != null)
            {
                //Resettime x 2 uit hexagon prefab
                hexChunk.StopAllCoroutines();
                hexChunk.StartCoroutine("SplitChunk", 20);
                hexChunk.MoveChildren(point, chargedRadius, dir);

                hasHit = true;

                continue;
            }

            Hexagon hex = objectsHit [n].GetComponent<Hexagon>();
            if (hex != null)
            {
                hex.MoveHexagon(point, chargedRadius, dir);
                
                hasHit = true;

                continue;
            } 

            if (dir == 1)
            {
                if (objectsHit [n].tag == "Player")
                {
                    Vector3 launchDir = Vector3.Normalize(objectsHit [n].transform.position - point);
                    launchDir.y = 1;
                    objectsHit [n].GetComponent<Player_Force>().AddImpact(launchDir, 100 * chargedRadius);
                } else if (objectsHit [n].tag == "PhysicsObject" || objectsHit [n].tag == "Flag")
                {
                    objectsHit [n].GetComponent<Rigidbody>().AddForce(Vector3.up * 500 * chargedRadius);
                }
            }
        }
            
        if (hasHit)
        {
            StartCoroutine(ShootTimer(reloadTime));
            StartCoroutine(PlayRubbleSound());
        }        
    }

    /*void ShootTerraformer(string objectHit, Vector3 point, int dir)
    {
        GameObject obj = GameObject.Find(objectHit);

        if (obj != null)
        {
            bool hasHit = false;
            Collider[] objectsHit = Physics.OverlapSphere(point, radius);

            for (int n = 0; n < objectsHit.Length; n++)
            {

                HexChunk hexChunk = objectsHit [n].gameObject.GetComponent<HexChunk>();
                if (hexChunk != null)
                {
                    //Resettime x 2 uit hexagon prefab
                    hexChunk.StopAllCoroutines();
                    hexChunk.StartCoroutine("SplitChunk", 10);
                    for (int c = 0; c < hexChunk.transform.childCount; c++)
                    {
                        Hexagon hex = hexChunk.transform.GetChild(c).gameObject.GetComponent<Hexagon>();
                        if (hex != null)
                        {
                            if (Vector3.Distance(hex.transform.position, point) < radius)
                            {
                                Vector2 hexXZ = new Vector2(hex.transform.position.x, hex.transform.position.z);
                                Vector2 pointXZ = new Vector2(point.x, point.z);
                            
                                float distance = (radius + 0.5f) - Vector2.Distance(hexXZ, pointXZ);
                                distance = distance > 0 ? Mathf.Pow(distance, 2) : 0;
                            
                                if (distance > 0)
                                {
                                    hasHit = true;
                                    Vector3 target = Vector3.up * distance * dir;
                                
                                    hex.StopAllCoroutines();
                                    hex.StartCoroutine("MoveTo", target);

                                    hex.transform.parent.GetComponent<HexChunk>().StopAllCoroutines();
                                    hex.transform.parent.GetComponent<HexChunk>().StartCoroutine("SplitChunk", 10);

                                    StartCoroutine(ShootTimer(reloadTime));
                                }
                            }
                        }
                    }

                    continue;
                }

                Hexagon hex2 = objectsHit [n].gameObject.GetComponent<Hexagon>();
                if (hex2 != null)
                {            
                    Vector2 hexXZ = new Vector2(hex2.transform.position.x, hex2.transform.position.z);
                    Vector2 pointXZ = new Vector2(point.x, point.z);
            
                    float distance = (radius + 0.5f) - Vector2.Distance(hexXZ, pointXZ);
                    //float distance = (radius + 0.5f) - Vector3.Distance(hex.transform.position, point);
                    distance = distance > 0 ? Mathf.Pow(distance, 2) : 0;

                    if (distance > 0)
                    {
                        hasHit = true;
                        Vector3 target = Vector3.up * distance * dir;
                
                        hex2.StopAllCoroutines();
                        hex2.StartCoroutine("MoveTo", target);

                        hex2.transform.parent.GetComponent<HexChunk>().StopAllCoroutines();
                        hex2.transform.parent.GetComponent<HexChunk>().StartCoroutine("SplitChunk", 10);

                        StartCoroutine(ShootTimer(reloadTime));
                    }
                }
            }
        
            if (hasHit)
            {
                StartCoroutine(ShootTimer(reloadTime));
                StartCoroutine(PlayRubbleSound());
            }
        }
    }*/

    /*void ShootTerraformer(string objectHit, Vector3 point, int dir)
    {
        GameObject obj = GameObject.Find(objectHit);
        
        if (obj != null)
        {       
            bool hasHit = false;
            Collider[] objectsHit = Physics.OverlapSphere(obj.transform.position, radius);
            for (int n = 0; n < objectsHit.Length; n++)
            {
                Hexagon hex = objectsHit [n].gameObject.GetComponent<Hexagon>();
                if (hex == null)
                    continue;
                
                Vector2 hexXZ = new Vector2(hex.transform.position.x, hex.transform.position.z);
                Vector2 pointXZ = new Vector2(obj.transform.position.x, obj.transform.position.z);
                
                float distance = (radius + 0.5f) - Vector2.Distance(hexXZ, pointXZ);
                //float distance = (radius + 0.5f) - Vector3.Distance(hex.transform.position, point);
                distance = distance > 0 ? Mathf.Pow(distance, 2) : 0;
                
                if (distance > 0)
                {
                    hasHit = true;
                    Vector3 target = Vector3.up * distance * dir;
                    
                    hex.StopAllCoroutines();
                    hex.StartCoroutine("MoveTo", target);
                    //hex.ChangeColor(Color.red);
                    
                    StartCoroutine(ShootTimer(reloadTime));
                }
            }
            
            if (hasHit)
            {
                StartCoroutine(ShootTimer(reloadTime));
                StartCoroutine(PlayRubbleSound());
            }
        }
    }*/

    //Waitforseconds moet gelijk zijn aan hoe lang de pilaren omhoog blijven.
    IEnumerator PlayRubbleSound()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/snd_terraformer_rubble");
        audioSource.PlayOneShot(audioClip);

        yield return new WaitForSeconds(5);

        audioSource.PlayOneShot(audioClip);
    }
}
