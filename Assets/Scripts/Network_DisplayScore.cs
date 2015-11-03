using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Network_DisplayScore : MonoBehaviour
{
    void Update()
    {
        DisplayScore();
    }

    void DisplayScore()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int p = 0; p < players.Length; p++)
        {
            Text scoreText = GameObject.Find("Score Text " + p).GetComponent<Text>();
            scoreText.text = "Score: " + players [p].GetComponent<Player_Score>().Score;
        }
    }
}
