using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    IEnumerator SplitChunk(float time)
    {
        GetComponent<MeshRenderer>().enabled = false;
        for (int c = 0; c < transform.childCount; c++)
            transform.GetChild(c).gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        GetComponent<MeshRenderer>().enabled = true;
        for (int c = 0; c < transform.childCount; c++)
            transform.GetChild(c).gameObject.SetActive(false);
    }
}
