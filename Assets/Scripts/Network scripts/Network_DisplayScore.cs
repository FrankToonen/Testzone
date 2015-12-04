using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Network_DisplayScore : MonoBehaviour
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
                    {
                        if (pName.Length > 10)
                        {
                            pName = pName.Remove(10);
                        } else
                        {
                            pName = pName.Remove(pName.Length - 1);
                        }
                    }

                    scoreText.GetComponent<Text>().text = pName + ": " + (int)players [p].GetComponent<Player_Score>().Score;
                    GameObject.Find("Scoreboard Overlay " + p).GetComponent<RawImage>().enabled = true;
                    GameObject.Find("Scoreboard Gem " + p).GetComponent<RawImage>().enabled = true;
                } else
                {
                    scoreText.GetComponent<Text>().text = "";
                    GameObject.Find("Scoreboard Overlay " + p).GetComponent<RawImage>().enabled = false;
                    GameObject.Find("Scoreboard Gem " + p).GetComponentInChildren<RawImage>().enabled = false;
                }
            }
        }
    }

    public void ShowScoreboard()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        for (int p = 0; p < 4; p++)
        {
            GameObject scoreText = GameObject.Find("Scoreboard Text " + p);
            if (scoreText != null)
            {
                if (p < players.Length)
                {
                    string pName = players [p].name;
                    if (pName != "")
                        pName = pName.Remove(pName.Length - 1);
                    
                    scoreText.GetComponent<Text>().text = pName + ": " + (int)players [p].GetComponent<Player_Score>().Score;
                } else
                {
                    scoreText.GetComponent<Text>().text = "";
                }
            }
        }
    }
}
