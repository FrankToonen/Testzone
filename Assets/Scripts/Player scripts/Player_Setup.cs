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

	public int playerNumber;

	[SyncVar]
	public string
		selectedGun;

	void Start ()
	{
		if (isLocalPlayer) {
			GetComponent<TP_Controller> ().enabled = true;
            
			cam.enabled = true;
			cam.GetComponent<TP_Camera> ().enabled = true;

			audioListener.enabled = true;

			gameObject.layer = 9; // "Player" layer

			//Cursor.visible = false;  

			selectedGun = GameObject.Find ("NetworkManager").GetComponent<Network_Manager> ().selectedGun;
			//Debug.Log(selectedGun);

			GetNetIdentity ();
			SetIdentity ();
		}

		GameObject.FindWithTag ("NetworkManager").GetComponent<Network_Manager> ().SetPlayerColor ();
	}

	void Update ()
	{
		if (transform.name == "" || transform.name == "Player(Clone)"
            /*|| transform.name == "PlayerTerraformer(Clone)" || transform.name == "PlayerMagnetGun(Clone)"*/) { // Aanpassen als prefabnaam verandert
			SetIdentity ();
		}
	}

	void AddGun ()
	{
		/*if (selectedGun == "Magnet gun")
        {
            gameObject.AddComponent<Gun_MagnetGun>();
            GetComponent<Player_Shoot>().primaryGun = GetComponent<Gun_MagnetGun>();
        } else if (selectedGun == "Terraformer")
        {*/
		gameObject.AddComponent<Gun_Terraformer> ();
		GetComponent<Player_Shoot> ().primaryGun = GetComponent<Gun_Terraformer> ();
		//}
	}
    
	[Command]
	void CmdSyncData (string name, string gun)
	{
		playerUniqueIdentity = name;
		selectedGun = gun;
	}
    
	[Client]
	void GetNetIdentity ()
	{
		CmdSyncData (MakeUniqueIdentity (), selectedGun);
	}
    
	void SetIdentity ()
	{
		transform.name = isLocalPlayer ? MakeUniqueIdentity () : playerUniqueIdentity;
		GameObject.FindWithTag ("NetworkManager").GetComponent<Network_DisplayScore> ().DisplayScore ();
		AddGun ();
	}
    
	string MakeUniqueIdentity ()
	{
		string uniqueName = GameObject.Find ("NetworkManager").GetComponent<Network_Manager> ().playername;
		if (uniqueName == "")
			uniqueName = "Player";
		uniqueName += playerNumber;
		return uniqueName;
	}
}
