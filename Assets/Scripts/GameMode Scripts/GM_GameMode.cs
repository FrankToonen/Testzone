using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_GameMode : MonoBehaviour
{
    public bool isServer;
    protected GM_Manager manager;
    protected GM_Bases_Manager basesManager;
    protected GM_Message messageManager;

    protected virtual void Start()
    {
        manager = GameObject.Find("GameModeManager").GetComponent<GM_Manager>();
        basesManager = GameObject.Find("GameModeManager").GetComponent<GM_Bases_Manager>();
        messageManager = GameObject.Find("GameModeManager").GetComponent<GM_Message>();
    }
}
