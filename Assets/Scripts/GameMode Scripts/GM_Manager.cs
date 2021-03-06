﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Manager : NetworkBehaviour
{
    GameMode gameMode = GameMode.BB;
    public enum GameMode
    {
        None, // Default
        CTF, // Capture the flag
        HP, // Hot potato
        BB, // "Basketball"
        KOTH // King of the hill
    }
    ;

    [SerializeField]
    RawImage
        scoreboardImage;

    public bool roundStarted { get; private set; }

    AudioSource audioSource;
    Text timerText;
    float timer;
    bool timerStarted, initialized, roundEnded;

    void Initialize()
    {
        audioSource = GetComponent<AudioSource>();

        /*string gm = GameObject.FindWithTag("NetworkManager").GetComponent<Network_Manager>().selectedGameMode;
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
            case "King of the Hill":
                {
                    gameMode = GM_Manager.GameMode.KOTH;
                    break;
                }
        }*/
                
        timerText = GameObject.Find("Timer Text").GetComponent<Text>();

        GameObject.FindWithTag("Hexgrid").GetComponent<HexGrid>().GenerateGrid();
        GetComponent<GM_Bases_Manager>().Initialize(gameMode);
        timer = 1;

        initialized = true;
    }

    void Update()
    {
        if (!RoundFinished && timerStarted && initialized)
        {
            UpdateTimer();
        } else if (RoundFinished && timerStarted && !roundEnded && initialized)
        {

        } else if (!initialized)
        {
            Initialize();
        }

        if (/*isServer &&*/ RoundFinished && initialized && !roundEnded)
        {
            RpcShowScoreboard();
            roundEnded = true;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                Player_Setup setup = p.GetComponent<Player_Setup>();
                if (setup != null)
                {
                    GetComponent<Player_Pause>().Pause(false);
                    setup.EnableControls(false);
                    setup.EnableArmMovement(false);
                    setup.EnableCameraMovement(false);
                }
            }
        }

        if (roundEnded)
        {
            if (NetworkServer.active || NetworkClient.active)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    GameObject.FindWithTag("NetworkManager").GetComponent<Network_HUD>().Disconnect();
                }
            }
        }
    }

    void UpdateTimer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, 600);

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

    public void SetGameMode(GameMode gm)
    {
        gameMode = gm;
    }

    [ClientRpc]
    void RpcShowScoreboard()
    {
        scoreboardImage.gameObject.SetActive(true);
        GameObject.FindWithTag("NetworkManager").GetComponent<Network_DisplayScore>().ShowScoreboard();

        RpcPlaySound("endround");
    }

    public void StartRound()
    {
        StartCoroutine(SpawnGameMode());
    }

    /*void SetScore()
    {
        if (gameMode == GameMode.BB)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                p.GetComponent<Player_Score>().ChangeScore(5);
            }
        }
    }*/

    IEnumerator SpawnGameMode()
    {
        yield return new WaitForSeconds(3);

        //SetScore();

        EnablePlayerMovement(false);

        // Countdown
        for (int i = 1; i < 11; i++)
        {
            int t = 11 - i;
            string ts = t.ToString();
            if (t < 10)
            {
                ts = "0" + ts;
            }

            RpcSetCountdownTimer(ts);
            if (11 - i == 3)
            {
                RpcPlaySound("countdown");
            }

            yield return new WaitForSeconds(1);
        }
        
        RpcSetCountdownTimer("start");
        //RpcPlaySound("startround");

        yield return new WaitForSeconds(.5f);

        RpcSetCountdownTimer("empty");
        roundStarted = true;

        EnablePlayerMovement(true);
        SpawnGameModeObject();

        RpcStartTimer(240);
    }

    [ClientRpc]
    void RpcSetCountdownTimer(string t)
    {
        RawImage timerImage = GameObject.Find("Countdown Timer Image").GetComponent<RawImage>();
        timerImage.texture = Resources.Load<Texture>("Images/HUD/countdown_" + t);
    }

    [ClientRpc]
    public void RpcStartTimer(int duration)
    {
        timer = duration;
        timerStarted = true;

        if (isServer)
        {
            GetComponent<GM_Bases_Manager>().SetStartBase();
        }
    }
    
    void EnablePlayerMovement(bool m)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject p in players)
        {
            p.GetComponent<Player_Setup>().RpcResetToStartRound(m);
        }
    }
    
    void SpawnGameModeObject()
    {
        GameObject gameModeObject = null;
        if (GM == GM_Manager.GameMode.HP || GM == GM_Manager.GameMode.BB)
        {
            gameModeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Ball") as GameObject, new Vector3(109, 150, 97), Quaternion.identity) as GameObject;
        } else if (GM == GM_Manager.GameMode.CTF)
        {
            gameModeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Flag") as GameObject, new Vector3(112, 23, 97), Quaternion.identity) as GameObject;
        }
        
        if (gameModeObject != null)
        {
            NetworkServer.Spawn(gameModeObject);
        }
    }

    [ClientRpc]
    public void RpcPlaySound(string name)
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/snd_" + name);
        audioSource.PlayOneShot(audioClip);
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
