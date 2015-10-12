using UnityEngine;
using System.Collections;

public class PulseGun : MonoBehaviour
{
	Rigidbody rigidBody;
	public GameObject player;
	public float force;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody> ();
	}

	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0)) {
			transform.position += new Vector3 (0, 0.5f, 0);
			Vector3 direction = Vector3.Normalize (player.transform.position - transform.position) * -force;
			rigidBody.AddForce (direction);
		}

		if (Input.GetMouseButtonDown (1)) {
			transform.position += new Vector3 (0, 0.5f, 0);
			Vector3 direction = Vector3.Normalize (player.transform.position - transform.position) * (force / 2);
			direction.y *= -1;
			rigidBody.AddForce (direction);
		}
	}
}
