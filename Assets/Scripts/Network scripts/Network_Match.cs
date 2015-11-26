using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Network_Match : MonoBehaviour
{
    NetworkManager manager;
    //Network_HUD hud;
    public UnityEngine.Networking.Match.MatchDesc match;

    void Start()
    {
        manager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        //hud = GameObject.FindWithTag("NetworkManager").GetComponent<Network_HUD>();
    }

    public void JoinMatch()
    {
        manager.matchName = match.name;
        manager.matchSize = (uint)match.currentSize;
        manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);

        //hud.findMatchButton.gameObject.SetActive(false);
        //hud.stopMMButton.gameObject.SetActive(false);
    }
}
