using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour
{
    public GameObject hexagon, hexChunk;
    public int width, length;
    //float hexYSize;
    GameObject[,] hexagons;
    GameObject[] chunks;
    Texture2D heightMap;
    Texture2D colorMap;

    // Use this for initialization
    void Start()
    {
        heightMap = Resources.Load<Texture2D>("Images/heightmap");
        colorMap = Resources.Load<Texture2D>("Images/colormap");

        hexagons = new GameObject[length, width];
        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < width; z++)
            {
                Renderer hex = hexagon.GetComponent<Renderer>();
                //hexYSize = hex.bounds.size.y;

                float x_pos = transform.position.x + x * hex.bounds.size.x + (z % 2 * (hex.bounds.size.x / 2));
                float y_pos = GetYPos(x, z);
                float z_pos = transform.position.z + z * (hex.bounds.size.z / 4 * 3);
                Vector3 pos = new Vector3(x_pos, y_pos, z_pos);

                GameObject newHex = Instantiate(hexagon, pos, hexagon.transform.rotation) as GameObject;
                newHex.transform.name = "hexagon" + x + z;
                newHex.GetComponent<Renderer>().material.color = colorMap.GetPixel(x, z);
                newHex.GetComponent<Hexagon>().Initialize(x, z, pos);
                hexagons [x, z] = newHex;
            }
        }

        CreateChunks();
    }

    public float GetYPos(int x, int z)
    {
        float y_pos = transform.position.y + (0.5f - heightMap.GetPixel(x, z).grayscale) * /*hexYSize*/ 10 * 3;
        return y_pos;
    }

    // Group hexagons in chunks of at most 10x10 hexagons
    void CreateChunks()
    {
        // Create array of all top chunks
        chunks = new GameObject[((length / 10) + 1) * ((width / 10) + 1)];

        // Loop over all hexagons per 10x10 area
        int index = 0;
        for (int i = 0; i < length; i+=10)
        {
            for (int j = 0; j < width; j+=10)
            {
                // Create a chunk for each 10x10 area and name it after its x & z positions
                GameObject chunk = Instantiate(hexChunk, Vector3.zero, Quaternion.identity) as GameObject;
                chunk.transform.name = "HexChunk" + i + j;
                chunk.transform.parent = transform;

                // Loop over each hexagon within this 10x10 area and change its parent to the newly created chunk
                for (int x = 0; x < 10; x++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (i + x < length && j + z < width)
                            hexagons [i + x, j + z].transform.parent = chunk.transform;
                    }
                }

                chunks [index] = chunk;
                GroupMaterials(chunk); 

                index++;
            }
        }
    }

    // Divide the chunk into chunks with corresponding materials to enable multiple materials within each chunk
    void GroupMaterials(GameObject c)
    {
        List<GameObject> hexes = new List<GameObject>();
        List<Material> materials = new List<Material>();
        Material currentMaterial = null;

        // Loop over every child in the given chunk and group them by material
        int childCount = c.transform.childCount;
        int hexesDivided = 0;
        while (hexesDivided < childCount)
        {
            for (int i = 0; i < c.transform.childCount; i++)
            {
                GameObject hex = c.transform.GetChild(i).gameObject;
                Material hexMaterial = hex.GetComponent<MeshRenderer>().material;

                // Set the material to match to and check if it hasn't been seen before
                if (currentMaterial == null && !materials.Contains(hexMaterial))
                {
                    currentMaterial = hexMaterial;
                    materials.Add(hexMaterial);
                }

                // If the hex material color matches the material, add it to the list.
                if (currentMaterial.color == hexMaterial.color)
                {
                    hexes.Add(hex);
                    hexesDivided++;
                }
            }

            // Create a new 'subchunk' to add the matched hexagon to.
            GameObject newChunk = Instantiate(hexChunk, Vector3.zero, Quaternion.identity) as GameObject;
            newChunk.transform.parent = c.transform;
            newChunk.transform.name = "hexChunk" + currentMaterial.color;
            foreach (GameObject h in hexes)
            {
                h.transform.parent = newChunk.transform;
            }
            CombineMeshes(newChunk);

            // Clear the list and material and repeat till the while loop is exited
            hexes.Clear();
            currentMaterial = null;
        }
    }

    // Combine the meshes within a given hexChunk (code mostly from unity documentation)
    void CombineMeshes(GameObject c)
    {
        MeshFilter[] meshFilters;
        CombineInstance[] combine;
            
        meshFilters = c.GetComponentsInChildren<MeshFilter>();   
        combine = new CombineInstance[meshFilters.Length - 1];
           
        int index = 0;
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters [i].sharedMesh == null)
                continue;

            combine [index].mesh = meshFilters [i].sharedMesh;
            combine [index++].transform = meshFilters [i].transform.localToWorldMatrix;
            meshFilters [i].gameObject.SetActive(false);
        }
            
        c.GetComponent<MeshFilter>().mesh = new Mesh();
        c.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        c.GetComponent<MeshCollider>().sharedMesh = c.GetComponent<MeshFilter>().mesh;
        c.GetComponent<MeshRenderer>().material = meshFilters [1].GetComponent<MeshRenderer>().material;
    }



    // TEST
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine("ChangeHeightMap", "heightmaptest");
        }
    }

    IEnumerator ChangeHeightMap(string name)
    {
        heightMap = Resources.Load<Texture2D>("Images/" + name);

        foreach (GameObject obj in chunks)
        {
            HexChunk hc = obj.GetComponent<HexChunk>();
            hc.MoveChildren();
            StartCoroutine("ReCombine", hc.gameObject);
            yield return null;
        }
    }

    IEnumerator ReCombine(GameObject c)
    {
        yield return new WaitForSeconds(5);
        GroupMaterials(c);
    }
}