using UnityEngine;
using System.Collections;

public class GM_Base_BB : GM_Base
{
    bool[] hasBeenActive;

    protected override void Start()
    {
        base.Start();

        //transform.position += new Vector3(0, 20, 0);

        hasBeenActive = new bool[4];
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.tag == "Ball")
            {
                string lastHit = GameObject.FindWithTag("Ball").GetComponent<GM_Ball>().lastHitBy;
                if (lastHit.Length > 0)
                {
                    lastHit = lastHit.Remove(lastHit.Length - 1);
                }

                GivePoints(100, lastHit + " has scored!");
                other.GetComponent<GM_Ball>().ResetPosition();

                manager.RpcPlaySound("score");
                //manager.RpcPlaySound("crowd");
            }
        }
    }

    public override void GivePoints(float points, string message)
    {
        if (whoseBase == null)
        {
            FindWhoseBase();
        }
        
        if (whoseBase != null && !manager.RoundFinished)
        {
            string lastHit = GameObject.FindWithTag("Ball").GetComponent<GM_Ball>().lastHitBy;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                float score = points;
                if (lastHit == p.name)
                {
                    if (lastHit == whoseBase.name)
                    {
                        score *= -1;
                    } else
                    {
                        score *= 2;
                    }
                } else if (p.name == whoseBase.name)
                {
                    score = (score * -1) / 2;
                }

                p.GetComponent<Player_Score>().ChangeScore(score);
            }

            basesManager.RpcPlayScoreParticles(playerNumber);
        }
        
        if (message != "")
        {
            messageManager.RpcShowMessage(message, 5);
        }
    }

    public override void SelectNewIndex()
    {
        int newIndex = Random.Range(0, 4);

        bool isFull = true;
        foreach (bool b in hasBeenActive)
        {
            isFull = isFull && b;
        }
        if (isFull == true)
        {
            return;
        }

        if (hasBeenActive [newIndex])
        {
            SelectNewIndex();
        } else
        {
            hasBeenActive [newIndex] = true;
            basesManager.RpcChangeBasePosition(newIndex);
        }
    }
}
