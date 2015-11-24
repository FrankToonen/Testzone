using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class expand : MonoBehaviour
{
    [SerializeField]
    GameObject[]
        bars = new GameObject[3];

    private GameObject player;
    private float width;

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

    public void ChangeBarsVisible(int amount)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i >= amount)
            {
                bars [i].transform.localScale = new Vector3(0, 6, 6);
                bars [i].GetComponent<Image>().color = Color.gray;
            } else
            {
                bars [i].transform.localScale = new Vector3(8.8f, 6, 6);
                bars [i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ChargeBar(int i, float r)
    {
        float ratio = Mathf.Clamp(r, 0, 1);
        bars [i].transform.localScale = new Vector3(8.8f * ratio, 6, 6);
    }
}
