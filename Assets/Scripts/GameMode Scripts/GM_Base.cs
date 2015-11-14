using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Base : GM_GameMode
{
    [SerializeField]
    int
        playerNumber;

    GameObject whoseBase;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        transform.name = "Base" + playerNumber;

        // Prachtige code
        Color baseColor = Color.white;
        switch (playerNumber)
        {
            case 0:
                {
                    baseColor = Color.red;
                    break;
                }
            case 1: 
                {
                    baseColor = Color.green;
                    break;
                }
            case 2: 
                {
                    baseColor = Color.blue;
                    break;
                }
            case 3: 
                {
                    baseColor = Color.yellow;
                    break;
                }
        }
        baseColor.a = 0.5f;
        GetComponent<Renderer>().material.color = baseColor;
    }

    void FindWhoseBase()
    {
        if (whoseBase == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > playerNumber)
            {
                whoseBase = players [playerNumber];
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (manager.GM != GM_Manager.GameMode.CTF)
        {
            return;
        }
        FindWhoseBase();

        if (whoseBase == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > playerNumber)
            {
                whoseBase = players [playerNumber];
            }
        }

        if (other.gameObject == whoseBase && isServer)
        {
            GM_Flag flag = other.gameObject.GetComponentInChildren<GM_Flag>();
            if (flag != null)
            {
                GivePoints(1);
                flag.CmdChangeFlagHolder("");
                flag.ResetPosition();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (manager.GM != GM_Manager.GameMode.HP || !manager.RoundFinished)
        {
            return;
        }

        FindWhoseBase();

        GM_Ball ball = other.GetComponent<GM_Ball>();
        if (ball != null)
        {
            GivePoints(-1);
            ball.Reset();
            manager.RpcStartTimer(30);
        }
    }

    public void GivePoints(int points)
    {
        if (whoseBase != null)
        {
            whoseBase.GetComponent<Player_Score>().ChangeScore(points);
        }
    }
}
