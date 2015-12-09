using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Networking;
using System.Collections;

public class Gun_Terraformer : Gun
{
    //[SerializeField]
    protected float radius;

    protected override void Start()
    {
        base.Start();

        primarySoundName = "terraformer_primary";
        secondarySoundName = "terraformer_secondary";
        reloadTime = 2;
        radius = 3;
        maxCharges = 3;
        charges = maxCharges;

        if (isPlayer)
        {
            uiCharges = GameObject.Find("Bar Overlap").GetComponent<Expand>();
            GetComponent<Player_Shoot>().EventShoot += Shoot;
        }
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
        float chargedRadius = radius * (Mathf.Floor(/*Mathf.Clamp(*/charge/*, 1, maxChargeTime)*/) + 1);

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
                    objectsHit [n].GetComponent<Player_Force>().AddImpact(launchDir, (100 / objectsHit [n].transform.localScale.x) * chargedRadius);
                } else if (objectsHit [n].tag == "PhysicsObject" || objectsHit [n].tag == "Flag" || objectsHit [n].tag == "Ball")
                {
                    /*float force = 0;
                    switch ((int)chargedRadius)
                    {
                        case 3:
                            {
                                force = 4000;
                                break;
                            }
                        case 6:
                            {
                                force = 6000;
                                break;
                            }
                        case 9:
                            {
                                force = 8000;
                                break;
                            }
                    }*/
                    objectsHit [n].GetComponent<Rigidbody>().AddForce(Vector3.up * (2000 + 2000 * (chargedRadius / 3)) /*force*/);

                    if (objectsHit [n].tag == "Ball")
                    {
                        objectsHit [n].GetComponent<GM_Ball>().lastHitBy = transform.name;
                    }
                }
            }
        }
            
        if (hasHit)
        {
            StartCoroutine(ShootTimer(reloadTime));
            StartCoroutine(PlayRubbleSound());
        }        
    }

    //Waitforseconds moet gelijk zijn aan hoe lang de pilaren omhoog blijven.
    IEnumerator PlayRubbleSound()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/snd_terraformer_rubble");
        audioSource.PlayOneShot(audioClip);

        yield return new WaitForSeconds(10);

        audioSource.PlayOneShot(audioClip);
    }
}
