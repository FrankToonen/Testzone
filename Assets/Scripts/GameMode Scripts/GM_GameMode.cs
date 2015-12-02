using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_GameMode : NetworkBehaviour
{
    protected GM_Manager manager;
    protected GM_Bases_Manager basesManager;

    protected virtual void Start()
    {
        manager = GameObject.Find("GameModeManager").GetComponent<GM_Manager>();
        basesManager = GameObject.Find("GameModeManager").GetComponent<GM_Bases_Manager>();
    }
}
