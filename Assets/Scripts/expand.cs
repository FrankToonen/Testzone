using UnityEngine;
using System.Collections;

public class expand : MonoBehaviour
{
    private GameObject player;
    private float width;

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.GetComponent<Player_Setup>().isLocalPlayer)
                {
                    player = p;
                }
            }
        } else
        {
            width = player.GetComponent<Gun_Terraformer>().Charge * 3;
            this.transform.localScale = new Vector3(width, 6, 6);
        }

    }
}
