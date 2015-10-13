using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public Gun[] guns;
	public Gun currentGun;

	// Use this for initialization
	void Start ()
	{
		EquipGun (1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i<guns.Length; i++) {
			if (Input.GetKeyDown ((i + 1).ToString ()) || Input.GetKeyDown ("[" + (i + 1) + "]")) {
				EquipGun (i);
				break;
			}
		}

		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
			currentGun.Shoot ();
		}
	}

	void EquipGun (int i)
	{
		currentGun = guns [i];
		currentGun.transform.parent = transform;
	}
}
