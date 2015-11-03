using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Network_Manager : NetworkManager
{
    Network_DisplayScore displayScore;
    public string playername { get; private set; }

    void Start()
    {
        displayScore = GetComponent<Network_DisplayScore>();
    }

    void Update()
    {
        if (IsClientConnected())
        {
            displayScore.DisplayScore();
        } else
        {
            GameObject inputField = GameObject.Find("Player Name");
            if (inputField != null)
            {
                playername = inputField.GetComponent<InputField>().text;
            }
        }
    }
}
