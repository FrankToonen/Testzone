using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Network_Manager : NetworkManager
{
    Network_DisplayScore displayScore;

    void Start()
    {
        displayScore = GetComponent<Network_DisplayScore>();
    }

    void Update()
    {
        if (IsClientConnected())
            displayScore.DisplayScore();
    }
}
