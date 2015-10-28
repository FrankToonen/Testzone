using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class HexGrid : MonoBehaviour
{
    public GameObject hexagon;
    public int width, length;
    GameObject[,] hexagons;
    public GameObject[] chunks;
    Texture2D heightMap;
    Texture2D colorMap;

    // Use this for initialization
    void Start()
    {
        heightMap = Resources.Load<Texture2D>("Materials/heightmap");
        //colorMap = Resources.Load<Texture2D>("Materials/colormap");

        hexagons = new GameObject[length, width];
        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < width; z++)
            {
                Renderer hex = hexagon.GetComponent<Renderer>();

                float x_pos = transform.position.x + x * hex.bounds.size.x + (z % 2 * (hex.bounds.size.x / 2));
                //float y_pos = (Mathf.Round(heightMap.GetPixel(x, z).grayscale * 100) / 100) * hex.bounds.size.y;
                float y_pos = heightMap.GetPixel(x, z).grayscale * hex.bounds.size.y * 5;
                float z_pos = transform.position.z + z * (hex.bounds.size.z / 4 * 3);
                Vector3 pos = new Vector3(x_pos, y_pos, z_pos);

                GameObject newHex = Instantiate(hexagon, pos, hexagon.transform.rotation) as GameObject;
                //newHex.transform.parent = transform;
                newHex.transform.name = "hexagon" + x + z;
                //newHex.GetComponent<Renderer>().material.color = colorMap.GetPixel(x, z);
                newHex.GetComponent<Hexagon>().Initialize();
                hexagons [x, z] = newHex;
            }
        }

        CreateChunks();
        CombineMeshes();
    }

    void CreateChunks()
    {
        chunks = new GameObject[((length / 10) + 1) * ((width / 10) + 1)];

        int index = 0;
        for (int i = 0; i < length; i+=10)
        {
            for (int j = 0; j < width; j+=10)
            {
                GameObject chunk = new GameObject();
                chunk.AddComponent<HexChunk>();
                chunk.AddComponent<MeshFilter>();
                chunk.AddComponent<MeshRenderer>();
                chunk.AddComponent<MeshCollider>();
                chunk.transform.name = "HexChunk" + i + j;

                for (int x = 0; x < 10; x++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (i + x < length && j + z < width)
                            hexagons [i + x, j + z].transform.parent = chunk.transform;
                    }
                }

                chunks [index] = chunk;
                index++;
            }
        }
    }

    void CombineMeshes()
    {
        MeshFilter[] meshFilters;
        CombineInstance[] combine;

        for (int i = 0; i < chunks.Length; i++)
        {
            if (chunks [i] == null)
                continue;

            meshFilters = chunks [i].GetComponentsInChildren<MeshFilter>();

            combine = new CombineInstance[meshFilters.Length - 1];

            int index = 0;
            for (int j = 0; j < meshFilters.Length; j++)
            {
                if (meshFilters [j].sharedMesh == null)
                    continue;

                combine [index].mesh = meshFilters [j].sharedMesh;
                combine [index++].transform = meshFilters [j].transform.localToWorldMatrix;
                meshFilters [j].gameObject.SetActive(false);
            }

            chunks [i].GetComponent<MeshFilter>().mesh = new Mesh();
            chunks [i].GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            chunks [i].GetComponent<MeshCollider>().sharedMesh = chunks [i].GetComponent<MeshFilter>().mesh;
            //chunks [i].GetComponent<MeshRenderer>().material = meshFilters [1].GetComponent<MeshRenderer>().material;

            //TEMP
            chunks [i].GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Brown");
        }
    }

    /*void Update()
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
    }*/
}