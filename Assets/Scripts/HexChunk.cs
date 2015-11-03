using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour
{
    IEnumerator SplitChunk(float time)
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        for (int c = 0; c < transform.childCount; c++)
            transform.GetChild(c).gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
        for (int c = 0; c < transform.childCount; c++)
            transform.GetChild(c).gameObject.SetActive(false);
    }
}
