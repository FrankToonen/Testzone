using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public GameObject background;
	public GameObject logo;
	public GameObject text;

	public bool isSkipped;
	public bool isSkippable;

	IEnumerator Skippable()
	{
		yield return new WaitForSeconds(2);
		isSkippable = true;
	}

	IEnumerator AutoSkip() 
	{
		yield return new WaitForSeconds(6);
		isSkipped = true;
	}
	
	void Update () {


		if (!isSkipped) 
		{
			background.SetActive (true);
			logo.SetActive (true);
			text.SetActive (true);
			StartCoroutine(Skippable());
			StartCoroutine(AutoSkip());
		} else 
		{
			Application.LoadLevel("Menu");
		}

		if (Input.anyKey && isSkippable) 
		{
			isSkipped = true;
		}
	}

}
