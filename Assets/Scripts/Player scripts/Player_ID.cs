using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_ID : NetworkBehaviour
{
    [SyncVar]
    string
        playerUniqueIdentity;

    NetworkInstanceId playerNetID;
    Transform myTransform;

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
    }

    // Use this for initialization
    void Awake()
    {
        myTransform = transform;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (myTransform.name == "" || myTransform.name == "Player(Clone)")
        {
            SetIdentity();
        }
    }

    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }

    [Client]
    void GetNetIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    void SetIdentity()
    {
        /*if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueIdentity;
        } else
        {
            myTransform.name = MakeUniqueIdentity();
        }*/

        myTransform.name = isLocalPlayer ? MakeUniqueIdentity() : playerUniqueIdentity;
    }

    string MakeUniqueIdentity()
    {
        string uniqueName = "Player" + playerNetID.ToString();
        return uniqueName;
    }
}
