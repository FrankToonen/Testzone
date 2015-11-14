using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Network_Manager : NetworkManager
{
    public string playername { get; private set; }
    //public string selectedGun { get; private set; }
    public string selectedGameMode { get; private set; }

    GM_Manager manager;

    void Update()
    {
        if (!IsClientConnected())
        {
            GameObject inputField = GameObject.Find("Player Name");
            if (inputField != null)
            {
                playername = inputField.GetComponent<InputField>().text;
            }

            /*GameObject gunDropDown = GameObject.Find("Gun Select");
            if (gunDropDown != null)
            {
                selectedGun = gunDropDown.GetComponent<Dropdown>().captionText.text;
            }*/

            GameObject gameModeDropDown = GameObject.Find("GameMode Select");
            if (gameModeDropDown != null)
            {
                selectedGameMode = gameModeDropDown.GetComponent<Dropdown>().captionText.text;
            }
        }
    }

    public void SetPlayerColor()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int p = 0; p < 4; p++)
        {
            if (p < players.Length)
            {
                Renderer renderer = players [p].GetComponentInChildren<Renderer>();
                players [p].GetComponent<Player_Setup>().playerNumber = p;

                switch (p)
                {
                    case 0:
                        {
                            renderer.material.color = Color.red;
                            break;
                        }
                    case 1:
                        {
                            renderer.material.color = Color.green;
                            break;
                        }
                    case 2:
                        {
                            renderer.material.color = Color.blue;
                            break;
                        }
                    case 3: 
                        {
                            renderer.material.color = Color.yellow;
                            break;
                        }
                }
            }
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        //
        // numplayers aanpassen naar == 3
        // disable player movement totdat de server vol zit
        //

        if (numPlayers == 1 && GameObject.Find("Ball") == null && GameObject .Find("Flag") == null) // Bij 4 spelers "numPlayers == 3"
        {
            manager = GameObject.Find("GameModeManager").GetComponent<GM_Manager>();
            StartCoroutine(SpawnGameMode());
        }
    }

    IEnumerator SpawnGameMode()
    {
        //
        // Start countdown?
        //

        yield return new WaitForSeconds(3);

        GameObject gameModeObject = null;
        if (manager.GM == GM_Manager.GameMode.HP)
        {
            gameModeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Ball") as GameObject, new Vector3(112, 23, 97), Quaternion.identity) as GameObject;
        } else if (manager.GM == GM_Manager.GameMode.CTF)
        {
            gameModeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Flag") as GameObject, new Vector3(112, 23, 97), Quaternion.identity) as GameObject;
        }
        NetworkServer.Spawn(gameModeObject);

        manager.RpcStartTimer(30);
    }
}
