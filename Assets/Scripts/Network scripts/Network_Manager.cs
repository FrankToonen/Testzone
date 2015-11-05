using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

//DisplayScore alleen aanroepen als deze verandert
//
//

public class Network_Manager : NetworkManager
{
    public string playername { get; private set; }
    public string selectedGun { get; private set; }

    void Update()
    {
        if (!IsClientConnected())
        {
            GameObject inputField = GameObject.Find("Player Name");
            if (inputField != null)
            {
                playername = inputField.GetComponent<InputField>().text;
            }

            GameObject dropDown = GameObject.Find("Gun Select");
            if (dropDown != null)
            {
                selectedGun = dropDown.GetComponent<Dropdown>().captionText.text;
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
                        renderer.material.color = Color.blue;
                        break;
                    case 1:
                        renderer.material.color = Color.red;
                        break;
                    case 2:
                        renderer.material.color = Color.green;
                        break;
                    case 3: 
                        renderer.material.color = Color.grey;
                        break;
                }
            }
        }
    }

    public void SetPlayerGuns()
    {

    }
}
