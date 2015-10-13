using UnityEngine;
using System.Collections;

public class Terraformer : Gun
{
	public float radius;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	public override void Shoot ()
	{
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); // Vanuit de speler
			if (Physics.Raycast (ray, out hit)) {
				//DEBUG
				Debug.DrawRay (ray.origin, ray.direction * 100, Color.blue, 10f);
				//

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
		}
	}
}
