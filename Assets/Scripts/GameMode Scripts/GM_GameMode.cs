using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_GameMode : NetworkBehaviour
{
    protected GM_Manager manager;

    // Use this for initialization
    protected virtual void Start()
    {
        manager = GameObject.Find("GameModeManager").GetComponent<GM_Manager>();
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}
