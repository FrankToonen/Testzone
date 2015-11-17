using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Manager : NetworkBehaviour
{
    GameMode gameMode = GameMode.None;
    public enum GameMode
    {
        None, // Default
        CTF, // Capture the flag
        HP, // Hot potato
        BB // "Basketball"
    }
    ;

    GameObject[] bases;

    Text timerText;
    float timer;
    bool timerStarted, initialized;

    void Initialize()
    {
        string gm = GameObject.FindWithTag("NetworkManager").GetComponent<Network_Manager>().selectedGameMode;
        switch (gm)
        {
            case "Hot potato":
                {
                    gameMode = GM_Manager.GameMode.HP;
                    break;
                }
            case "Capture the Flag":
                {
                    gameMode = GM_Manager.GameMode.CTF;
                    break;
                }
            case "Basketball":
                {
                    gameMode = GM_Manager.GameMode.BB;
                    break;
                }
        }

        bases = new GameObject[4];
        for (int i  = 0; i < 4; i++)
        {
            bases [i] = GameObject.Find("Base" + i);
            bases [i].GetComponent<GM_Base>().GetPosition();
        }
        
        timerText = GameObject.Find("Timer Text").GetComponent<Text>();
        initialized = true;
    }

    void Update()
    {
        if (!RoundFinished && timerStarted && initialized)
        {
            UpdateTimer();
        } else if (!initialized)
        {
            Initialize();
        }
    }

    void UpdateTimer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, 120);

        int min = (int)(timer / 60);

        int seconds = (int)(timer - (min * 60));
        string sec = seconds.ToString();
        if (sec.Length == 1)
        {
            sec = "0" + sec;
        }

        float miliSeconds = timer - (min * 60) - seconds;
        string mSec = (miliSeconds + " ").Substring(2);
        if (mSec.Length > 2)
        {
            mSec = mSec.Remove(2);
        } else
        {
            mSec = "00";
        }

        timerText.text = "TIME " + min + ":" + sec + ":" + mSec;

        timerStarted = timer > 0;
    }

    [ClientRpc]
    public void RpcStartTimer(int duration)
    {
        timer = duration;
        timerStarted = true;
    }

    public void SetGameMode(GameMode gm)
    {
        gameMode = gm;
    }

    public GameMode GM
    {
        get { return gameMode;}
    }

    public bool RoundFinished
    {
        get { return timer == 0; }
    }
}
