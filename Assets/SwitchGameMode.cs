using UnityEngine;
using System.Collections;

public class SwitchGameMode : MonoBehaviour {

    public Vector3 PushUp = new Vector3(0, 1, 0);

    public GameObject GameModeStandard;
    public Vector3 GameModeStandardWanted;
    public Transform GameModeStandardHidden;

    public GameObject GameModeCTF;
    public Vector3 GameModeCTFWanted;
    public GameObject GameModeCTFHidden;

	// Use this for initialization
	void Start () {
        GameModeStandard.transform.position = GameModeStandardHidden.transform.position;
        GameModeStandardWanted = new Vector3(0, 1, 0);

        GameModeCTF.transform.position = GameModeStandardHidden.transform.position;
        GameModeCTFWanted = new Vector3(0, 2, 0);
	}

    // Update is called once per frame
    void Update() {
        CTF();
	}

    void StandardLevel()
    {
        if (GameModeStandard.transform.position.y < GameModeStandardWanted.y)
        {
            GameModeStandard.transform.position += PushUp * Time.deltaTime;
        }
    }

    void CTF()
    {
        if (GameModeCTF.transform.position.y < GameModeCTFWanted.y)
        {
            GameModeCTF.transform.position += PushUp * Time.deltaTime;
        }
    }
}
