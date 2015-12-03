using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Player_Pause : MonoBehaviour
{
    ButtonContainer buttons;
    GameObject overlay;
    Player_Setup setup;
    bool isPaused;

    void Awake()
    {
        //setup = GetComponent<Player_Setup>();
        overlay = GameObject.Find("Pause Overlay");
        FindButtonContainer();
        Pause(false);
    }

    void FindButtonContainer()
    {
        GameObject found = GameObject.Find("ButtonContainer");
        if (found != null)
        {
            buttons = found.GetComponent<ButtonContainer>();
        }
    }

    void FindLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                setup = p.GetComponent<Player_Setup>();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Pause(isPaused);
        }
    }

    public void Pause(bool p)
    {
        overlay.SetActive(p);
        EnableButtons(p);

        if (setup == null)
        {
            FindLocalPlayer();
        }

        if (setup != null)
        {        
            setup.EnableControls(!p);
            setup.EnableArmMovement(!p);
            setup.EnableCameraMovement(!p);
        }
    }
    
    void EnableButtons(bool e)
    {
        buttons.FindButton("Quit Button").SetActive(e);
        buttons.FindButton("HowToPlay Button").SetActive(e);
        buttons.FindButton("Continue Button").SetActive(e);
    }

    void OpenPauseMenu()
    {
        isPaused = false;
        Pause(isPaused);
    }
}
