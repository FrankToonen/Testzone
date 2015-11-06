using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class Hexagon : MonoBehaviour
{
	public float resetTime;
	Vector3 startPosition;
	float maxHeight, minHeight, moveSpeed;

	// Use this for initialization
	/*void Start()
    {
        startPosition = transform.position;
        maxHeight = transform.position.y + 4;
        minHeight = transform.position.y - 4;
        moveSpeed = 0.1f;

        //int r = Random.Range(0, 100);
        //if (r > 85)
        //    GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Brown");
    }*/

	public void Initialize ()
	{
		startPosition = transform.position;
		maxHeight = transform.position.y + 8;
		minHeight = transform.position.y - 8;
		moveSpeed = 0.2f;
	}

	public void MoveHexagon (Vector3 point, float radius, int dir)
	{
		Vector2 hexXZ = new Vector2 (transform.position.x, transform.position.z);
		Vector2 pointXZ = new Vector2 (point.x, point.z);
            
		float distance = (radius + 0.5f) - Vector2.Distance (hexXZ, pointXZ);
		distance = distance > 0 ? Mathf.Pow (distance, 2) : 0;
            
		if (distance > 0) {
			//hasHit = true;
			Vector3 target = Vector3.up * distance * dir;
                
			StopAllCoroutines ();
			StartCoroutine ("MoveTo", target);

			transform.parent.GetComponent<HexChunk> ().StopAllCoroutines ();
			transform.parent.GetComponent<HexChunk> ().StartCoroutine ("SplitChunk", 20);
                
			//StartCoroutine(ShootTimer(reloadTime));
		}
	}

	public IEnumerator MoveTo (Vector3 target)
	{
		Vector3 targetPosition = transform.position + target;
		targetPosition.y = Mathf.Clamp (targetPosition.y, minHeight, maxHeight);

		while (Mathf.Abs(transform.position.y - targetPosition.y) > 0.05f && transform.position.y <= maxHeight && transform.position.y >= minHeight) {
			float newY = transform.position.y;

			int dir = targetPosition.y > startPosition.y ? 1 : -1;
			float min = startPosition.y > targetPosition.y ? targetPosition.y : transform.position.y; // : startPosition.y
			float max = startPosition.y <= targetPosition.y ? targetPosition.y : transform.position.y; // : startPosition.y
			newY = Mathf.Clamp (newY + /*target.y*/ dir * moveSpeed, min, max);

			transform.position = new Vector3 (transform.position.x, newY, transform.position.z);

			yield return null;
		}

		transform.position = targetPosition;

		yield return new WaitForSeconds (resetTime);

		StartCoroutine ("ResetY", startPosition - transform.position);
	}

	IEnumerator ResetY (Vector3 target)
	{
		while (Mathf.Abs(transform.position.y - startPosition.y) > 0.05f) {
			float newY = transform.position.y;

			int dir = target.y > 0 ? 1 : -1;
			float min = startPosition.y > transform.position.y ? transform.position.y : startPosition.y;
			float max = startPosition.y <= transform.position.y ? transform.position.y : startPosition.y;
			newY = Mathf.Clamp (newY + dir * moveSpeed, min, max);

			//Vector3 pos = transform.position;
			//pos.y += (startPosition.y - transform.position.y) > 0 ? moveSpeed : -moveSpeed;
			//transform.position = pos;

			transform.position = new Vector3 (transform.position.x, newY, transform.position.z);

			yield return null;
		}

		transform.position = startPosition;
		//ChangeColor(Color.white);
	}



	//DEBUG
	public void ChangeColor (Color color)
	{
		Renderer renderer = GetComponent<Renderer> ();
		renderer.material.color = color;
	}



	//TEST
	public IEnumerator SinWave (Vector3 direction, float delay, Color color)
	{
		yield return new WaitForSeconds (delay);
		//ChangeColor(color);
		StartCoroutine ("MoveTo", direction);
	}
}
