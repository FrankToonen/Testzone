using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Network_DisplayScore : NetworkBehaviour
{
    public void DisplayScore()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int p = 0; p < 4; p++)
        {
            GameObject scoreText = GameObject.Find("Score Text " + p);
            if (scoreText != null)
            {
                if (p < players.Length)
                {
                    scoreText.GetComponent<Text>().text = players [p].name + ": " + players [p].GetComponent<Player_Score>().Score;
                } else
                {
                    scoreText.GetComponent<Text>().text = "";
                }
            }
        }
    }
}
