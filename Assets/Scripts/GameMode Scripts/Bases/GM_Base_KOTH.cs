using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Base_KOTH : GM_Base
{
    public Vector3[] positions;
    int currentIndex = 0;
    float timeLeft, rotationTime;

    protected override void Start()
    {
        base.Start();
        positions = new Vector3[5];
        positions [0] = new Vector3(110.5f, 19.5f, 95.5f); // Centre
        positions [1] = new Vector3(87.5f, 10, 161);
        positions [2] = new Vector3(35.5f, 10, 75);
        positions [3] = new Vector3(134.5f, 10, 29.5f);
        positions [4] = new Vector3(186, 10, 116);

        transform.position = positions [0];
        rotationTime = 30;
        timeLeft = rotationTime;
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (manager.roundStarted)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                SelectRandomPosition();
                timeLeft = rotationTime;
            }
        }
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

    public void ChangeIndex(int i)
    {
        currentIndex = i;
        transform.position = positions [i];
    }

    void SelectRandomPosition()
    {
        int i = (int)Random.Range(0, 5);

        if (i == currentIndex)
        {
            SelectRandomPosition();
        } else
        {
            manager.RpcChangeBasePosition(i);
        }
    }
}
