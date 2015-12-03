using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    Function
        functionCalled;

    public enum Function
    {
        StartHost,
        StartClient,
        StartMatchmaker,
        StopMatchmaker,
        CreateMatch,
        FindMatch,
        Disconnect
    }
    ;

    Network_HUD networkHUD;
    Button button;

    public void Initialize()
    {        
        networkHUD = GameObject.Find("NetworkManager").GetComponent<Network_HUD>();

        button = GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            PressButton();
        });
    }

    public void PressButton()
    {
        switch (functionCalled)
        {
            case Function.StartHost:
                {
                    networkHUD.StartHost();
                    break;
                }
            case Function.StartClient:
                {
                    networkHUD.StartClient();
                    break;
                }
            case Function.StartMatchmaker:
                {
                    networkHUD.StartMatchmaker();
                    break;
                }
            case Function.StopMatchmaker:
                {
                    networkHUD.StopMatchmaker();
                    break;
                }
            case Function.CreateMatch:
                {
                    networkHUD.CreateMatch();
                    break;
                }
            case Function.FindMatch:
                {
                    networkHUD.FindMatch();
                    break;
                }
            case Function.Disconnect:
                {
                    networkHUD.Disconnect();
                    break;
                }
        }
    }
}
