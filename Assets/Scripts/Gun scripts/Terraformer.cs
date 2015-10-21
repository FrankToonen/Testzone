using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class Terraformer : Gun
{
    // Schiet alleen als je iets raakt
    // Reload dan ook alleen
    // En maakt dan ook pas geluid

    [SerializeField]
    float
        radius;

    protected override void Start()
    {
        base.Start();
        reloadTime = 1;

        //Temp reloadbar
        reloadBar = GameObject.Find("Reload Bar Terraformer").GetComponent<Image>();
        startScale = reloadBar.transform.localScale;
        targetScale = new Vector3(0, startScale.y, 0);
        reloadBar.transform.localScale = targetScale;
        //
    }

    protected override void ShootPrimary(string objectHit)
    {
        ShootTerraformer(objectHit, 1);
    }

    protected override void ShootSecondary(string objectHit)
    {
        ShootTerraformer(objectHit, -1);
    }

    void ShootTerraformer(string objectHit, int dir)
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
                //Playsound
            }
        }
    }
}
