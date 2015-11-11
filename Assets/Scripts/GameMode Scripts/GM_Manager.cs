using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GM_Manager : MonoBehaviour
{
    GameMode gameMode;
    public enum GameMode
    {
        CTF, // Capture the flag
        HP // Hot potato
    }
    ;

    Text timerText;
    float timer, timerDuration;

    // Use this for initialization
    void Start()
    {
        timerText = GameObject.Find("Timer Text").GetComponent<Text>();
        timerDuration = 10;
        timer = timerDuration;

        gameMode = GameMode.HP;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!RoundFinished)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, 120);

        int minutes = (int)(timer / 60);

        int seconds = (int)(timer - (minutes * 60));
        string s = seconds.ToString();
        if (s.Length == 1)
        {
            s = "0" + s;
        }

        float miliSeconds = timer - (minutes * 60) - seconds;
        string mS = (miliSeconds + " ").Substring(2);
        if (mS.Length > 2)
        {
            mS = mS.Remove(2);
        } else
        {
            mS = "00";
        }

        timerText.text = "TIME " + minutes + ":" + s + ":" + mS;
    }

    public void ResetTimer()
    {
        timer = timerDuration;
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
