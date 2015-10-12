using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour
{
	public GameObject hexagon;
	public int width, length;
	public float radius;
	GameObject[,] hexagons;

	// Use this for initialization
	void Start ()
	{
		hexagons = new GameObject[length, width];
		for (int x = 0; x < length; x++) {
			for (int z = 0; z < width; z++) {
				Renderer hex = hexagon.GetComponent<Renderer> ();
				Vector3 pos = new Vector3 (transform.position.x + x * hex.bounds.size.x + (z % 2 * (hex.bounds.size.x / 2)), /*z % 2 * .5f*/0, transform.position.z + z * (hex.bounds.size.z / 4 * 3));
				GameObject newHex = Instantiate (hexagon, pos, hexagon.transform.rotation) as GameObject;
				newHex.transform.parent = transform;
				hexagons [x, z] = newHex;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
				Vector3 point = hit.transform.position;
				Collider[] objectsHit = Physics.OverlapSphere (point, radius);
				for (int n = 0; n < objectsHit.Length; n++) {
					Hexagon hex = objectsHit [n].gameObject.GetComponent<Hexagon> ();
					if (hex == null)
						continue;

					float distance = radius - Vector3.Distance (hex.transform.position, point);
					distance = distance > 0 ? Mathf.Pow (distance, 2) : 0;
					Vector3 target = Vector3.up * distance;
					if (Input.GetMouseButtonDown (1))
						target *= -1;

					if (distance > 0) {
						hex.StopAllCoroutines ();

						hex.StartCoroutine ("MoveTo", target);
						if (Input.GetMouseButtonDown (0))
							hex.ChangeColor (Color.red);
						else
							hex.ChangeColor (Color.green);
					}
				}
			}
		}*/

		if (Input.GetKeyDown (KeyCode.B)) {
			foreach (Transform child in transform) {
				Hexagon hex = child.gameObject.GetComponent<Hexagon> ();
				hex.StopAllCoroutines ();
				hex.StartCoroutine (hex.SinWave (Vector3.up, Vector3.Distance (new Vector3 (15, 0, 15), hex.transform.position) / 4, Color.red));
			}
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			foreach (Transform child in transform) {
				Hexagon hex = child.gameObject.GetComponent<Hexagon> ();
				hex.StopAllCoroutines ();
				hex.StartCoroutine (hex.SinWave (-Vector3.up, Vector3.Distance (new Vector3 (15, 0, 15), hex.transform.position) / 4, Color.green));
			}
		}
	}
}