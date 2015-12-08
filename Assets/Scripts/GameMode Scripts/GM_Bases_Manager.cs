using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Bases_Manager : NetworkBehaviour
{
    GM_Manager manager;
    GameObject[] bases;
    GameObject[] particles;

    float timeLeft, rotationTime;

    public void Initialize(GM_Manager.GameMode gameMode)
    {
        manager = GetComponent<GM_Manager>();

        bases = new GameObject[4];
        particles = new GameObject[4];
        for (int i  = 0; i < 4; i++)
        {
            particles [i] = GameObject.Find("ArenaFire" + i);
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
            
            bases [i].GetComponent<GM_GameMode>().isServer = isServer;
        }

        rotationTime = 60;
        timeLeft = rotationTime;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (manager == null || !isServer)
        {
            return;
        }

        if (manager.GM == GM_Manager.GameMode.BB || manager.GM == GM_Manager.GameMode.KOTH)
        {
            if (manager.roundStarted)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    bases [0].GetComponent<GM_Base>().SelectNewIndex();
                    timeLeft = rotationTime;
                }
            }
        }
    }

    public void SetStartBase()
    {
        bases [0].GetComponent<GM_Base>().SelectNewIndex();
        for (int i = 0; i < bases.Length; i++)
        {
            if (bases [i].activeInHierarchy)
            {
                RpcPlayScoreParticles(i);
            }
        }
    }

    [ClientRpc]
    public void RpcChangeBasePosition(int i)
    {
        if (manager.GM == GM_Manager.GameMode.KOTH)
        {
            bases [0].GetComponent<GM_Base_KOTH>().ChangeIndex(i);
        } else if (manager.GM == GM_Manager.GameMode.BB)
        {
            GameObject shields = GameObject.Find("Shields");
            for (int j = 0; j < bases.Length; j++)
            {
                particles [j].SetActive(j == i);
                bases [j].SetActive(/*j == i*/false);
                shields.transform.GetChild(j).gameObject.SetActive(j == i);
            }
            bases [i].SetActive(true);

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int j = 0; j < players.Length; j++)
            {
                int scale = i == j ? 4 : 2;
                //players [j].transform.localScale = new Vector3(scale, scale, scale);
                StartCoroutine(LerpScale(players [j], new Vector3(scale, scale, scale)));
            }
        }

        if (isServer)
        {
            manager.RpcPlaySound("basechange");
        }
    }

    [ClientRpc]
    public void RpcPlayScoreParticles(int i)
    {
        bases [i].GetComponent<GM_Base>().PlayScoreParticles();
    }

    IEnumerator LerpScale(GameObject obj, Vector3 targetScale)
    {
        while (Mathf.Abs(targetScale.x - obj.transform.localScale.x) > 0.1f)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, targetScale, Time.deltaTime);
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }
}
