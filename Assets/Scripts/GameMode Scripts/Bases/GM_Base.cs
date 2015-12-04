using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Base : GM_GameMode
{
    [SerializeField]
    protected int
        playerNumber;

    protected ParticleSystem[] scoreParticles;
    protected GameObject whoseBase;

    protected override void Start()
    {
        base.Start();

        playerNumber = name [name.Length - 1] - 48;

        scoreParticles = new ParticleSystem[4];
        for (int i = 0; i < 4; i++)
        {
            scoreParticles [i] = GameObject.Find("ArenaScoreParticle" + playerNumber.ToString() + i.ToString()).GetComponent<ParticleSystem>();
        }

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
        baseColor.a = 0.75f;
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

    public virtual void SelectNewIndex()
    {

    }

    public virtual void GivePoints(float points, string message = "")
    {
        if (whoseBase == null)
        {
            FindWhoseBase();
        }

        if (whoseBase != null && !manager.RoundFinished)
        {
            whoseBase.GetComponent<Player_Score>().ChangeScore(points);
            basesManager.RpcPlayScoreParticles(playerNumber);
        }

        if (message != "")
        {
            messageManager.RpcShowMessage(message, 5);
        }
    }

    public void PlayScoreParticles()
    {
        foreach (ParticleSystem ps in scoreParticles)
        {
            ps.Play();
        }
    }
}
