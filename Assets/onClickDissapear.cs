using UnityEngine;
using System.Collections;

public class onClickDissapear : MonoBehaviour {

    public GameObject howToPulseImage;
    public GameObject howToTerraformImage;

	void Update () {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            if (howToPulseImage.activeSelf == true)
            {
                howToPulseImage.SetActive(false);
                howToTerraformImage.SetActive(true);
            }
            else
            {
                howToTerraformImage.SetActive(false);
                howToPulseImage.SetActive(true);
            }
        }
        else if(Input.GetKey(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
	}
}
