using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Setup : NetworkBehaviour
{
    [SerializeField]
    Camera
        cam;

    [SerializeField]
    AudioListener
        audioListener;

    [SyncVar]
    string
        playerUniqueIdentity;
    
    NetworkInstanceId playerNetID;

    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<TP_Controller>().enabled = true;
            
            cam.enabled = true;
            cam.GetComponent<TP_Camera>().enabled = true;

            audioListener.enabled = true;

            gameObject.layer = 9; // "Player" layer

            //Cursor.visible = false;   
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
    }

    void Update()
    {
        if (transform.name == "" || transform.name == "PlayerTerraformer(Clone)" || transform.name == "PlayerMagnetGun(Clone)") // Aanpassen als prefabnaam verandert
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
        transform.name = isLocalPlayer ? MakeUniqueIdentity() : playerUniqueIdentity;
    }
    
    string MakeUniqueIdentity()
    {
        string uniqueName = GameObject.Find("NetworkManager").gameObject.GetComponent<Network_Manager>().playername;
        if (uniqueName == "")
            uniqueName = "Player" + playerNetID.ToString();
        return uniqueName;
    }
}
