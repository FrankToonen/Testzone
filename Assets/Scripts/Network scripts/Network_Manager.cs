using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Network_Manager : NetworkManager
{
    public string playername { get; private set; }
    //public string selectedGun { get; private set; }
    public string selectedGameMode { get; private set; }

    GameObject[] players;
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

        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            for (int p = 0; p < 4; p++)
            {
                if (p < players.Length)
                {
                    Debug.Log(players [p].name);
                    Debug.Log("Pulsegun times shot: " + players [p].GetComponent<Gun_PulseGun>().timesShot);
                    Debug.Log("Terraformer times shot: " + players [p].GetComponent<Gun_Terraformer>().timesShot);
                    Debug.Log("Score: " + players [p].GetComponent<Player_Score>().Score);
                }
            }

            Application.Quit();
        }*/
    }

    public void SetPlayerColor()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int p = 0; p < 4; p++)
        {
            if (p < players.Length)
            {
                if (players [p].GetComponent<Player_Setup>().playerNumber != p || players [p].transform.FindChild("Bot") == null)
                {
                    players [p].GetComponent<Player_Setup>().playerNumber = p;

                    switch (p)
                    {
                        case 0:
                            {
                                GameObject model = Instantiate(Resources.Load<GameObject>("Prefabs/bot_red"), Vector3.zero, Quaternion.identity) as GameObject;
                                model.name = "Bot";
                                model.transform.parent = players [p].transform;
                                model.transform.localPosition = new Vector3(0, -1.25f, 0);
                                model.transform.Rotate(Vector3.up, 180);
                                model.transform.localScale = new Vector3(13, 13, 13);
                                break;
                            }
                        case 1:
                            {
                                GameObject model = Instantiate(Resources.Load<GameObject>("Prefabs/bot_green"), Vector3.zero, Quaternion.identity) as GameObject;
                                model.name = "Bot";
                                model.transform.parent = players [p].transform;
                                model.transform.localPosition = new Vector3(0, -1.25f, 0);
                                model.transform.Rotate(Vector3.up, 180);
                                model.transform.localScale = new Vector3(13, 13, 13);
                                break;
                            }
                        case 2:
                            {
                                GameObject model = Instantiate(Resources.Load<GameObject>("Prefabs/bot_blue"), Vector3.zero, Quaternion.identity) as GameObject;
                                model.name = "Bot";
                                model.transform.parent = players [p].transform;
                                model.transform.localPosition = new Vector3(0, -1.25f, 0);
                                model.transform.Rotate(Vector3.up, 180);
                                model.transform.localScale = new Vector3(13, 13, 13);
                                break;
                            }
                        case 3: 
                            {
                                GameObject model = Instantiate(Resources.Load<GameObject>("Prefabs/bot_yellow"), Vector3.zero, Quaternion.identity) as GameObject;
                                model.name = "Bot";
                                model.transform.parent = players [p].transform;
                                model.transform.localPosition = new Vector3(0, -1.25f, 0);
                                model.transform.Rotate(Vector3.up, 180);
                                model.transform.localScale = new Vector3(13, 13, 13);
                                break;
                            }
                    }
                }
            }
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        manager = GameObject.Find("GameModeManager").GetComponent<GM_Manager>();
        if (numPlayers == 3 && !manager.roundStarted) // Bij 4 spelers "numPlayers == 3"
        {
            manager.StartRound();
        }
    }
}
