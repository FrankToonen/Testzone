using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Nametag : NetworkBehaviour
{
    GameObject localPlayer;

    void Update()
    {
        if (localPlayer == null)
        {
            FindLocalPlayer();
        } else
        {
            transform.LookAt(localPlayer.transform.FindChild("Camera").transform.position);
            transform.Rotate(Vector3.up, 180);

            float distance = Vector3.Distance(transform.position, localPlayer.transform.position) / 100;
            distance = Mathf.Clamp(distance, 0.15f, 0.5f);
            transform.localScale = new Vector3(distance, distance, distance);
        }
    }

    void FindLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayer = p;
            }
        }
    }
}
