using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Network/NetworkManagerHUD")]
[RequireComponent(typeof(NetworkManager))]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class Network_HUD : MonoBehaviour
{
    NetworkManager manager;
    ButtonContainer buttons;

    /*[SerializeField]
    public bool
        showGUI = true;*/
    [SerializeField]
    public int
        offsetX;
    [SerializeField]
    public int
        offsetY;

    //public Button hostButton, clientButton, startMMButton, createMatchButton, findMatchButton, stopMMButton;
    public GameObject joinButton;
    List<GameObject> joinButtons;

    // Runtime variable
    //bool showServer = false;
    
    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        joinButtons = new List<GameObject>();

        FindButtonContainer();
        //ResetButtons();
    }

    void FindButtonContainer()
    {
        GameObject found = GameObject.Find("ButtonContainer");
        if (found != null)
        {
            buttons = found.GetComponent<ButtonContainer>();
        }
    }

    public void ResetButtons()
    {
        buttons.FindButton("Host Button").SetActive(true);
        buttons.FindButton("Client Button").SetActive(true);
        buttons.FindButton("Start Matchmaking Button").SetActive(true);
        buttons.FindButton("Create Match Button").SetActive(false);
        buttons.FindButton("Find Match Button").SetActive(false);
        buttons.FindButton("Stop Matchmaking Button").SetActive(false);
    }

    void Update()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            return;
        }

        if (buttons == null)
        {
            FindButtonContainer();
        }

        ListMatches();

        /*if (!showGUI)
            return;
        
        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                manager.StartServer();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                manager.StartHost();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                manager.StartClient();
            }
        }
        if (NetworkServer.active && NetworkClient.active)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                manager.StopHost();
            }
        }*/
    }

    public void StartHost()
    {
        manager.StartHost();
        manager.GetComponent<Network_Manager>().SwitchSong("playing", 0.01f);
    }

    public void StartClient()
    {
        manager.StartClient();
    }

    public void StartMatchmaker()
    {
        manager.StartMatchMaker();

        if (buttons == null)
        {
            Debug.Log("Buttons == null");
            return;
        }

        buttons.FindButton("Host Button").SetActive(false);
        buttons.FindButton("Client Button").SetActive(false);
        buttons.FindButton("Start Matchmaking Button").SetActive(false);
        buttons.FindButton("Create Match Button").SetActive(true);
        buttons.FindButton("Find Match Button").SetActive(true);
        buttons.FindButton("Stop Matchmaking Button").SetActive(true);
    }

    public void CreateMatch()
    {
        if (manager.matchInfo == null)
        {
            if (manager.matches == null)
            {
                manager.matchName = "Default";
                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
            }
        }
    }

    public void FindMatch()
    {
        if (manager.matchInfo == null)
        {
            if (manager.matches == null)
            {
                manager.matchMaker.ListMatches(0, 20, "", manager.OnMatchList);
                ListMatches();
            }
        }
    }

    public void ListMatches()
    {
        if (manager.matchInfo == null)
        {
            if (manager.matches != null)
            {
                float yPos = 100;
                float xPos = 0;
                int counter = 0;
                foreach (var match in manager.matches)
                {
                    if (joinButtons.Count <= counter)
                    {
                        GameObject button = Instantiate(joinButton, Vector3.zero, Quaternion.identity) as GameObject;
                        button.transform.parent = GameObject.FindWithTag("Canvas").transform;
                        button.transform.localPosition = new Vector3(xPos, yPos, 0);
                        button.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                        button.GetComponent<Network_Match>().match = match;
                        button.GetComponent<Button>().GetComponentInChildren<Text>().text = "Join: " + match.name;
                        joinButtons.Add(button);
                    }

                    yPos += 70;
                    counter++;
                }

                buttons.FindButton("Create Match Button").gameObject.SetActive(manager.matches.Count == 0);
            }
        }
    }

    public void StopMatchmaker()
    {
        manager.StopMatchMaker();

        buttons.FindButton("Host Button").gameObject.SetActive(true);
        buttons.FindButton("Client Button").gameObject.SetActive(true);
        buttons.FindButton("Start Matchmaking Button").gameObject.SetActive(true);
        buttons.FindButton("Create Match Button").gameObject.SetActive(false);
        buttons.FindButton("Find Match Button").gameObject.SetActive(false);
        buttons.FindButton("Stop Matchmaking Button").gameObject.SetActive(false);
    }

    public void Disconnect()
    {
        manager.StopHost();
        
        manager.GetComponent<Network_Manager>().SwitchSong("menu", 0.05f);
    }

    /*void OnGUI()
    {
        if (!showGUI)
            return;
        
        int xpos = 10 + offsetX;
        int ypos = 40 + offsetY;
        int spacing = 24;
        
        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host"))
            {
                manager.StartHost();
            }
            ypos += spacing;
            
            if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client"))
            {
                manager.StartClient();
            }
            //manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
            ypos += spacing;
            
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only"))
            {
                manager.StartServer();
            }
            ypos += spacing;
        } else
        {
            if (NetworkServer.active)
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
                ypos += spacing;
            }
            if (NetworkClient.active)
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                ypos += spacing;
            }
        }
        
        if (NetworkClient.active && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
            {
                ClientScene.Ready(manager.client.connection);
                
                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }
        
        if (NetworkServer.active || NetworkClient.active)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop"))
            {
                manager.StopHost();
            }
            ypos += spacing;
        }
        
        if (!NetworkServer.active && !NetworkClient.active)
        {
            ypos += 10;
            
            if (manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker"))
                {
                    manager.StartMatchMaker();
                }
                ypos += spacing;
            } else
            {
                if (manager.matchInfo == null)
                {
                    if (manager.matches == null)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                        {
                            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
                        }
                        ypos += spacing;
                        
                        GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                        manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                        ypos += spacing;
                        
                        ypos += 10;
                        
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                        {
                            manager.matchMaker.ListMatches(0, 20, "", manager.OnMatchList);
                        }
                        ypos += spacing;
                    } else
                    {
                        foreach (var match in manager.matches)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                            {
                                manager.matchName = match.name;
                                manager.matchSize = (uint)match.currentSize;
                                manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                            }
                            ypos += spacing;
                        }
                    }
                }
                
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                {
                    showServer = !showServer;
                }
                if (showServer)
                {
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                    {
                        manager.SetMatchHost("localhost", 1337, false);
                        showServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                    {
                        manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                        showServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                    {
                        manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                        showServer = false;
                    }
                }
                
                ypos += spacing;
                
                GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
                ypos += spacing;
                
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                {
                    manager.StopMatchMaker();
                    hostButton.gameObject.SetActive(true);
                    clientButton.gameObject.SetActive(true);
                    startMMButton.gameObject.SetActive(true);
                }
                ypos += spacing;
            }
        }
    }*/
}
