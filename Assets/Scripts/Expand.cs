using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Expand : MonoBehaviour
{
    enum Shape
    {
        Bar,
        Square
    }
    ;

    [SerializeField]
    Shape
        shape;

    [SerializeField]
    GameObject[]
        bars;

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
            Scale();
        }
    }

    void Scale()
    {
        if (shape == Shape.Bar)
        {
            width = player.GetComponent<Gun_Terraformer>().Charge / 3;
            this.transform.localScale = new Vector3(width, 1, 1);
        }
    }

    public void ChangeBarsVisible(int amount)
    {
        for (int i = 0; i < bars.Length; i++)
        {
            if (i >= amount)
            {
                if (shape == Shape.Bar)
                {
                    bars [i].transform.localScale = new Vector3(0, 1, 1);
                } else if (shape == Shape.Square)
                {
                    bars [i].transform.localScale = new Vector3(0, 0, 0);
                }

                bars [i].GetComponent<Image>().color = Color.gray;
            } else
            {
                bars [i].transform.localScale = new Vector3(1, 1, 1);
                bars [i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ChargeBar(int i, float r)
    {
        float ratio = Mathf.Clamp(r, 0, 1);

        if (shape == Shape.Bar)
        {
            bars [i].transform.localScale = new Vector3(ratio, 1, 1);
        } else if (shape == Shape.Square)
        {
            bars [i].transform.localScale = new Vector3(ratio, ratio, ratio);
        }
    }
}
