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
        BB, // "Basketball"
        KOTH // King of the hill
    }
    ;

    public bool roundStarted { get; private set; }

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
            case "King of the Hill":
                {
                    gameMode = GM_Manager.GameMode.KOTH;
                    break;
                }
        }

        bases = new GameObject[4];
        for (int i  = 0; i < 4; i++)
        {
            bases [i] = GameObject.Find("Base" + i);

            switch (gameMode)
            {
                case GM_Manager.GameMode.BB:
                    {
                        bases [i].AddComponent<GM_Base_BB>();
                        break;
                    }
                case GM_Manager.GameMode.HP:
                    {
                        // Vorm aanpassen aan volledige gebied speler
                        bases [i].AddComponent<GM_Base_HP>();
                        break;
                    }
                case     GM_Manager.GameMode.CTF:
                    {
                        bases [i].AddComponent<GM_Base_CTF>();
                        break;
                    }
                case GM_Manager.GameMode.KOTH:
                    {
                        if (i == 0)
                        {
                            bases [i].AddComponent<GM_Base_KOTH>();
                        } else
                        {
                            bases [i].gameObject.SetActive(false);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
        timerText = GameObject.Find("Timer Text").GetComponent<Text>();

        GameObject.FindWithTag("Hexgrid").GetComponent<HexGrid>().GenerateGrid();

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
    public void RpcChangeBasePosition(int i)
    {
        if (GM == GameMode.KOTH)
        {
            bases [0].GetComponent<GM_Base_KOTH>().ChangeIndex(i);
        }
    }

    public void StartRound()
    {
        StartCoroutine(SpawnGameMode());
    }

    IEnumerator SpawnGameMode()
    {
        yield return new WaitForSeconds(1);

        EnablePlayerMovement(false);

        // Countdown
        for (int i = 1; i < 11; i++)
        {
            int t = 11 - i;
            RpcSetCountdownTimer(t.ToString());
            yield return new WaitForSeconds(1);
        }
        
        RpcSetCountdownTimer("Start");
        yield return new WaitForSeconds(.5f);
        RpcSetCountdownTimer("");
        roundStarted = true;

        EnablePlayerMovement(true);
        SpawnGameModeObject();

        RpcStartTimer(300);
    }

    [ClientRpc]
    void RpcSetCountdownTimer(string t)
    {
        Text timerText = GameObject.Find("Countdown Timer Text").GetComponent<Text>();
        timerText.text = t;
    }

    [ClientRpc]
    public void RpcStartTimer(int duration)
    {
        timer = duration;
        timerStarted = true;
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
            gameModeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Ball") as GameObject, new Vector3(112, 23, 97), Quaternion.identity) as GameObject;
        } else if (GM == GM_Manager.GameMode.CTF)
        {
            gameModeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Flag") as GameObject, new Vector3(112, 23, 97), Quaternion.identity) as GameObject;
        }
        
        if (gameModeObject != null)
        {
            NetworkServer.Spawn(gameModeObject);
        }
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
