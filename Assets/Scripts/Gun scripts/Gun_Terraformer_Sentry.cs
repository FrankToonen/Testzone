using UnityEngine;
using System.Collections;

public class Gun_Terraformer_Sentry : Gun_Terraformer
{
    public bool primary = true;
    public float shootCharge = 1;

    protected override void Start()
    {
        isPlayer = false;

        base.Start();

        reloadTime = 10;
        radius = 3;
        maxCharges = 1;
        charges = maxCharges;

        float scale = radius * shootCharge * 1.5f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (charges > 0)
        {
            Shoot("", transform.position, shootCharge, primary);
        }
    }
}
