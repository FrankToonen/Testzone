using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Score : NetworkBehaviour
{
    [SyncVar (hook = "SyncScore")]
    float
        score;
	
    void Update()
    {
        if (!isLocalPlayer)
            return;
    }

    public void ChangeScore(float amount)
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
    void CmdProvideScoreToServer(float s)
    {
        score = s;
    }

    [Client]
    void SyncScore(float s)
    {
        score = s;
        GameObject.FindWithTag("NetworkManager").GetComponent<Network_DisplayScore>().DisplayScore();
    }

    public float Score
    {
        get { return score; }
    }
}
