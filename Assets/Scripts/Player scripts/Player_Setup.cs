using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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

    public int playerNumber;

    Vector3 startPosition;
    /*[SyncVar]
    public string
        selectedGun;*/

    void Start()
    {
        startPosition = transform.position;
        if (isLocalPlayer)
        {
            transform.FindChild("Nametag").gameObject.SetActive(false);
            GetComponent<TP_Controller>().enabled = true;
            
            cam.enabled = true;
            cam.GetComponent<TP_Camera>().enabled = true;

            audioListener.enabled = true;

            gameObject.layer = 9; // "Player" layer

            //Cursor.visible = false;  

            //selectedGun = GameObject.Find("NetworkManager").GetComponent<Network_Manager>().selectedGun;
            //Debug.Log(selectedGun);

            GetNetIdentity();
            SetIdentity();
        }

        GameObject.FindWithTag("NetworkManager").GetComponent<Network_Manager>().SetPlayerColor();
    }

    void Update()
    {
        if (transform.name == "" || transform.name == "Player(Clone)")
        { // Aanpassen als prefabnaam verandert
            SetIdentity();
        }
    }

    [ClientRpc]
    public void RpcResetToStartRound(bool m)
    {
        if (isLocalPlayer)
        {
            // Reset position
            if (startPosition != Vector3.zero)
            {
                transform.position = startPosition;
            } else
            {
                startPosition = transform.position;
            }

            // Set movement bool
            GetComponent<TP_Controller>().enabled = m;
            GetComponent<Player_Shoot>().enabled = m;
        }
    }

    /* void AddGun()
    {
        if (selectedGun == "Magnet gun")
        {
            gameObject.AddComponent<Gun_MagnetGun>();
            GetComponent<Player_Shoot>().primaryGun = GetComponent<Gun_MagnetGun>();
        } else if (selectedGun == "Terraformer")
        {
        if (GetComponent<Gun_Terraformer>() == null)
        {
            gameObject.AddComponent<Gun_Terraformer>();
            GetComponent<Player_Shoot>().primaryGun = GetComponent<Gun_Terraformer>();
        }
        }
    }*/
    
    [Command]
    void CmdSyncData(string name/*, string gun*/)
    {
        playerUniqueIdentity = name;
        //selectedGun = gun;
    }
    
    [Client]
    void GetNetIdentity()
    {
        CmdSyncData(MakeUniqueIdentity()/*, selectedGun*/);
    }
    
    void SetIdentity()
    {
        transform.name = isLocalPlayer ? MakeUniqueIdentity() : playerUniqueIdentity;
        if (transform.name.Length != 0)
        {
            transform.FindChild("Nametag").GetComponent<TextMesh>().text = transform.name.Remove(transform.name.Length - 1);
        }

        GameObject.FindWithTag("NetworkManager").GetComponent<Network_DisplayScore>().DisplayScore();
        //AddGun();
    }
    
    string MakeUniqueIdentity()
    {
        string uniqueName = GameObject.Find("NetworkManager").GetComponent<Network_Manager>().playername;
        if (uniqueName == "")
        {
            List<string> names = new List<string>() { "Henk", "Arend", "Hans", "Ronald" };
            uniqueName = names [playerNumber];
        }
        uniqueName += playerNumber;
        return uniqueName;
    }
}
