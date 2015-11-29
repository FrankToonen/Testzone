using UnityEngine;
using System.Collections;

public class GM_Base_BB : GM_Base
{
    protected override void Start()
    {
        base.Start();
        transform.position += new Vector3(0, 25, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.tag == "Ball")
            {
                GivePoints(-1);
                other.GetComponent<GM_Ball>().ResetPosition();
            }
        }
    }
}
