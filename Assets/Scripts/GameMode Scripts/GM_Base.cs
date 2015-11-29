using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Base : GM_GameMode
{
    [SerializeField]
    protected int
        playerNumber;

    protected GameObject whoseBase;

    protected override void Start()
    {
        base.Start();

        playerNumber = name [name.Length - 1] - 48;
        GetColor();
    }

    void GetColor()
    {
        Color baseColor = Color.white;
        switch (playerNumber)
        {
            case 0:
                {
                    baseColor = Color.red;
                    if (manager.GM == GM_Manager.GameMode.KOTH)
                    {
                        baseColor = Color.cyan;
                    }
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

    protected void FindWhoseBase()
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

    /*
    void OnTriggerEnter(Collider other)
    {
        if (manager.GM != GM_Manager.GameMode.CTF && manager.GM != GM_Manager.GameMode.BB)
        {
            return;
        }

        FindWhoseBase();

        if (manager.GM == GM_Manager.GameMode.CTF)
        {
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
        } else if (manager.GM == GM_Manager.GameMode.BB)
        {
            if (isServer)
            {
                if (other.tag == "Ball")
                {
                    GivePoints(-1);
                    other.GetComponent<GM_Ball>().ResetPosition();
                }
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
            ball.ResetPosition();
            manager.RpcStartTimer(30);
        }
    }*/

    public void GivePoints(float points)
    {
        if (whoseBase != null)
        {
            whoseBase.GetComponent<Player_Score>().ChangeScore(points);
        }
    }
}
