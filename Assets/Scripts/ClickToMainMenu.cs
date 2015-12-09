using UnityEngine;
using System.Collections;

public class ClickToMainMenu : MonoBehaviour
{
    public ButtonContainer buttons;
    void Update()
    {
        if (Input.anyKey)
        {
            buttons.FindButton("Host Button").SetActive(true);
            buttons.FindButton("Client Button").SetActive(true);
            buttons.FindButton("Start Matchmaking Button").SetActive(true);
            buttons.FindButton("How to play Button").SetActive(true);
            buttons.FindButton("Quit Button").SetActive(true);
            GameObject.Find("Rotating Bots").GetComponent<RotateBots>().Activate();

            this.gameObject.SetActive(false);
        }
    }
}
