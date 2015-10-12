using UnityEngine;
using System.Collections;

public class TerrainModifyScript : MonoBehaviour
{
	TerrainData data;
	int xRes, zRes;
	float[,] heights;

	// Use this for initialization
	void Start ()
	{
		data = GetComponent<TerrainCollider> ().terrainData;
		xRes = data.heightmapWidth;
		zRes = data.heightmapHeight;
		heights = data.GetHeights (0, 0, xRes, zRes);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				RaiseTerrain (hit.point);
			}
		}
	}

	Vector3 CastRayToWorld ()
	{
		RaycastHit hit;
		Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit);
		return hit.transform.position;    
	}

	void RaiseTerrain (Vector3 point)
	{
		int mouseX = (int)(point.x / data.size.x) * xRes;
		int mouseZ = (int)(point.z / data.size.z) * zRes;
		float[,] modHeights = new float[1, 1];

		float y = heights [mouseX, mouseZ];
		y += 0.1f * Time.deltaTime;
		if (y > data.size.y)
			y = data.size.y;

		modHeights [0, 0] = y;
		heights [mouseX, mouseZ] = y;
		data.SetHeights (mouseX, mouseZ, modHeights);
	}
}
