using UnityEngine;
using System.Collections;

public class RotateBots : MonoBehaviour
{
    GameObject[] bots;
    Quaternion rotation;
    float timePassed, botTimer;
    int bot;
    bool active;

    void Start()
    {
        bots = new GameObject[4];
        for (int i = 0; i < transform.childCount; i++)
        {
            bots [i] = transform.GetChild(i).gameObject;
        }
    }
	
    void Update()
    {
        if (active)
        {
            timePassed += Time.deltaTime;
            botTimer -= Time.deltaTime;

            if (botTimer <= 0)
            {
                Activate();
            }

            rotation = Quaternion.Euler(0, timePassed * 10, 0);
            bots [bot].transform.rotation = rotation;

            //bot = (int)(((int)timePassed % 40) / 10);
        }
    }

    void SetBotActive(int i)
    {
        for (int j = 0; j < bots.Length; j++)
        {
            bots [j].SetActive(i == j);
        }

        botTimer = 10;
    }

    public void Activate()
    {
        bot = Random.Range(0, 4);
        SetBotActive(bot);
        active = true;
    }

    public void Deactivate()
    {
        active = false;
        SetBotActive(-1);
    }
}
