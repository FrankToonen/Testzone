using UnityEngine;
using System.Collections;

public class expand : MonoBehaviour {

    private GameObject player;
    private float width;

	// Update is called once per frame
	void Update () {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else {
                width = player.GetComponent<Gun_Terraformer>().Charge * 3;
                this.transform.localScale = new Vector3(width, 6, 6);
        }

        }
    }
