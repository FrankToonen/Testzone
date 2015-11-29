using UnityEngine;
using System.Collections;

public class GM_Base_HP : GM_Base
{
    void OnTriggerStay(Collider other)
    {
        if (manager.GM != GM_Manager.GameMode.HP || !manager.RoundFinished)
        {
            return;
        }
        
        FindWhoseBase();
        
        GM_Ball ball = other.GetComponent<GM_Ball>();
        if (ball != null)
        {
            GivePoints(-1);
            ball.ResetPosition();
            manager.RpcStartTimer(30);
        }
    }
}
