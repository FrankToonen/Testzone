using UnityEngine;
using System.Collections;

public class GM_Base_CTF : GM_Base
{
    void OnTriggerEnter(Collider other)
    {
        FindWhoseBase();

        if (other.gameObject == whoseBase && isServer)
        {
            GM_Flag flag = other.gameObject.GetComponentInChildren<GM_Flag>();
            if (flag != null)
            {
                GivePoints(1);
                flag.CmdChangeFlagHolder("");
                flag.ResetPosition();
            }
        }

    }
}
