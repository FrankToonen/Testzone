using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class HexGrid : MonoBehaviour
{
    public GameObject hexagon;
    public int width, length;
    public float radius;
    GameObject[,] hexagons;

    // Use this for initialization
    void Start()
    {
        hexagons = new GameObject[length, width];
        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < width; z++)
            {
                Renderer hex = hexagon.GetComponent<Renderer>();
                Vector3 pos = new Vector3(transform.position.x + x * hex.bounds.size.x + (z % 2 * (hex.bounds.size.x / 2)), 0, transform.position.z + z * (hex.bounds.size.z / 4 * 3));
                GameObject newHex = Instantiate(hexagon, pos, hexagon.transform.rotation) as GameObject;
                newHex.transform.parent = transform;
                hexagons [x, z] = newHex;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (Transform child in transform)
            {
                Hexagon hex = child.gameObject.GetComponent<Hexagon>();
                hex.StopAllCoroutines();
                hex.StartCoroutine(hex.SinWave(Vector3.up, Vector3.Distance(new Vector3(15, 0, 15), hex.transform.position) / 4, Color.red));
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (Transform child in transform)
            {
                Hexagon hex = child.gameObject.GetComponent<Hexagon>();
                hex.StopAllCoroutines();
                hex.StartCoroutine(hex.SinWave(-Vector3.up, Vector3.Distance(new Vector3(15, 0, 15), hex.transform.position) / 4, Color.green));
            }
        }
    }
}