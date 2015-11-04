using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CTF_Base : NetworkBehaviour
{
    [SerializeField]
    int
        playerNumber;

    GameObject whoseBase;

    // Use this for initialization
    void Start()
    {
        transform.name = "Base" + playerNumber;


        // Prachtige code
        Color baseColor = Color.white;
        switch (playerNumber)
        {
            case 0:
                baseColor = Color.blue;
                break;
            case 1: 
                baseColor = Color.red;
                break;
            case 2: 
                baseColor = Color.green;
                break;
            case 3: 
                baseColor = Color.grey;
                break;
        }
        baseColor.a = 0.5f;
        GetComponent<Renderer>().material.color = baseColor;
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (whoseBase == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > playerNumber)
                whoseBase = players [playerNumber];
        }

        if (other.gameObject == whoseBase && isServer)
        {
            CTF_Flag flag = other.gameObject.GetComponentInChildren<CTF_Flag>();
            if (flag != null)
            {
                whoseBase.GetComponent<Player_Score>().ChangeScore(1);
                flag.CmdChangeFlagHolder("");
                flag.ResetPosition();
            }
        }
    }
}
