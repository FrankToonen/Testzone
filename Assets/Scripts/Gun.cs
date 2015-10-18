using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    [Command]
    public virtual void CmdShoot()
    {
    }
}
