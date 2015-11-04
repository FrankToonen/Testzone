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
                    string pName = players [p].name;
                    if (pName != "")
                        pName = pName.Remove(pName.Length - 1);

                    scoreText.GetComponent<Text>().text = pName + ": " + players [p].GetComponent<Player_Score>().Score;
                } else
                {
                    scoreText.GetComponent<Text>().text = "";
                }
            }
        }
    }
}
