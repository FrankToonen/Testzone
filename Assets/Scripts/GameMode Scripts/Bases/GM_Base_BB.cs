using UnityEngine;
using System.Collections;

public class GM_Base_BB : GM_Base
{
    bool[] hasBeenActive;

    protected override void Start()
    {
        base.Start();

        if (isServer)
        {
            transform.position += new Vector3(0, 29, 0);
        }

        hasBeenActive = new bool[4];
    }

    void OnTriggerEnter(Collider other)
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
