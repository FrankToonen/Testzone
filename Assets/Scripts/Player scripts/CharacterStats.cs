using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {
    
    public Camera MainCamera;

    //CTF GAME MODE
    public GameObject myZone;
    private float myPointsCTF = 0;
    public GameObject flagPouch;
    public bool hasFlag = false;

	// Use this for initialization
	void Start () {

    }
	

	// Update is called once per frame
	void Update () {
        //CTF GAME MODE
          if (hasFlag == true ) 
        {
            MainCamera.GetComponent<TextMesh>().text = "You are the carrier";

        }
        else
        {
            MainCamera.GetComponent<TextMesh>().text = "Capture the flag!";
        }



    }

    void OnTriggerStay(Collider Zone)
    {
        if (Zone.transform.position == myZone.transform.position && hasFlag == true)
        {
            Debug.Log("In My zone");
            myPointsCTF = myPointsCTF + 1 * Time.deltaTime;
            Debug.Log("my points are: " + myPointsCTF);
        }
    }

}
