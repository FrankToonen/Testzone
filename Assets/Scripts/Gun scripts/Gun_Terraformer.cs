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

        uiCharges = GameObject.Find("Bar Overlap").GetComponent<expand>();

        primarySoundName = "terraformer_primary";
        secondarySoundName = "terraformer_secondary";
        reloadTime = 2;
        radius = 3;
        maxCharges = 3;
        charges = maxCharges;

        GetComponent<Player_Shoot>().EventShoot += Shoot;
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
        float chargedRadius = radius * Mathf.Floor(Mathf.Clamp(charge, 1, maxChargeTime));

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
                    objectsHit [n].GetComponent<Player_Force>().AddImpact(launchDir, 50 * chargedRadius);
                } else if (objectsHit [n].tag == "PhysicsObject" || objectsHit [n].tag == "Flag" || objectsHit [n].tag == "Ball")
                {
                    objectsHit [n].GetComponent<Rigidbody>().AddForce(Vector3.up * 1000 * chargedRadius);
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
