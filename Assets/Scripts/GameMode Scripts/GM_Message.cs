using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Message : NetworkBehaviour
{
    [SerializeField]
    GameObject
        overlay;

    void Update()
    {
        // TEST
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("H pressed");
            RpcShowMessage("Lol", 3);
        }
    }

    [ClientRpc]
    public void RpcShowMessage(string message, float time)
    {
        StartCoroutine(ShowMessage(message, time));
    }

    IEnumerator ShowMessage(string message, float time)
    {
        overlay.SetActive(true);
        overlay.GetComponentInChildren<Text>().text = message;

        yield return new WaitForSeconds(time);
        
        overlay.GetComponentInChildren<Text>().text = "";
        overlay.SetActive(false);
    }
}
