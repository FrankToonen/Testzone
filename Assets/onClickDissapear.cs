using UnityEngine;
using System.Collections;

public class onClickDissapear : MonoBehaviour {


	void Update () {
        if (Input.anyKey)
        {
            this.gameObject.SetActive(false);
        }
	}
}
