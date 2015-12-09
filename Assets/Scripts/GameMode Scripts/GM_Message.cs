using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Message : NetworkBehaviour
{
    [SerializeField]
    GameObject
        overlay;

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
