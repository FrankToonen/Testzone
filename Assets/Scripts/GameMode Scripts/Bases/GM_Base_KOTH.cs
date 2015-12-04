using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Base_KOTH : GM_Base
{
    public Vector3[] positions;
    int currentIndex = 0;

    protected override void Start()
    {
        base.Start();
        positions = new Vector3[5];
        positions [0] = new Vector3(110.5f, 19.5f, 95.5f); // Centre
        positions [1] = new Vector3(87.5f, 10, 161);
        positions [2] = new Vector3(35.5f, 10, 75);
        positions [3] = new Vector3(134.5f, 10, 29.5f);
        positions [4] = new Vector3(186, 10, 116);

        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = positions [0];
    }

    void OnTriggerStay(Collider other)
    {
        if (!manager.roundStarted)
        {
            return;
        }

        if (other.tag == "Player")
        {
            whoseBase = other.gameObject;
            GivePoints(0.1f);
            whoseBase = null;
        }
    }

    public override void GivePoints(float points)
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
    }

    public void ChangeIndex(int i)
    {
        currentIndex = i;
        transform.position = positions [i];
    }

    public override void SelectNewIndex()
    {
        int newIndex = (int)Random.Range(0, 5);

        if (newIndex == currentIndex)
        {
            SelectNewIndex();
        } else
        {
            basesManager.RpcChangeBasePosition(newIndex);
        }
    }
}
