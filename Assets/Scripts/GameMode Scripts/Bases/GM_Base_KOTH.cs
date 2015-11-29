using UnityEngine;
using System.Collections;

public class GM_Base_KOTH : GM_Base
{
    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(110.5f, 19.5f, 95.5f);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            whoseBase = other.gameObject;
            GivePoints(0.1f);
            whoseBase = null;
        }
    }
}
