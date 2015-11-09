using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HexChunk : MonoBehaviour
{
    public void MoveChildren(Vector3 point, float radius, int dir)
    {
        for (int c = 0; c < transform.childCount; c++)
        {
            Hexagon hex = transform.GetChild(c).GetComponent<Hexagon>();
            if (hex != null)
            {
                hex.MoveHexagon(point, radius, dir);
            }
        }
    }

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

    public void MoveChildren()
    {
        bool shouldDestroy = false;

        for (int c = transform.childCount -1; c >= 0; c--)
        {
            Transform child = transform.GetChild(c);
            HexChunk chunk = child.GetComponent<HexChunk>();
            if (chunk != null)
            {
                chunk.MoveChildren();
            } else
            {
                Hexagon hex = child.GetComponent<Hexagon>();
                hex.gameObject.SetActive(true);

                Vector3 hexPos = hex.transform.position;
                HexGrid grid = GameObject.FindWithTag("Hexgrid").GetComponent<HexGrid>();
                hexPos.y = grid.GetYPos(hex.xValue, hex.zValue);
                hex.SetPositions(hexPos);
                hex.transform.parent = transform.parent;

                shouldDestroy = true;
            }
        }

        if (shouldDestroy)
        {
            Destroy(gameObject);
        }
    }
}
