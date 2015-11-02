using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Score : NetworkBehaviour
{
    [SyncVar (hook = "SyncScore")]
    int
        score;
	
    void Update()
    {
        if (!isLocalPlayer)
            return;

        //Temp input
        if (Input.GetKeyDown(KeyCode.J))
            ChangeScore(10);
    }

    public void ChangeScore(int amount)
    {
        score += amount;
        TransmitScore();
    }

    [ClientCallback]
    void TransmitScore()
    {
        if (isLocalPlayer)
        {
            CmdProvideScoreToServer(score);
        }
    }

    [Command]
    void CmdProvideScoreToServer(int s)
    {
        score = s;
    }

    [Client]
    void SyncScore(int s)
    {
        score = s;
    }

    public int Score
    {
        get { return score; }
    }
}
