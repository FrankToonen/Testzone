using UnityEngine;
using System.Collections;

public class cursor : MonoBehaviour
{

    CursorLockMode wanted;

    // Use this for initialization
    void Start()
    {
        wanted = CursorLockMode.Locked;
    }

    void SetCursor()
    {
        Cursor.lockState = wanted;
        Cursor.visible = false;
    }

    void Update()
    {
        //SetCursor ();
    }
}
