using UnityEngine;
using System.Collections;

public class GM_Base_KOTH : GM_Base
{

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            GivePoints(0.1f);
        }
    }
}
