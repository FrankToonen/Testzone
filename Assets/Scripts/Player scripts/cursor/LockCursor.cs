using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour
{
    CursorLockMode wanted;
    bool visible;

    void Start()
    {
        wanted = CursorLockMode.None;
        visible = true;
    }

    /*void SetCursor()
    {
        Cursor.lockState = wanted;
        Cursor.visible = visible;
    }*/

    void Update()
    {
        Cursor.lockState = wanted;
        Cursor.visible = visible;
    }

    public void SetLockState(bool locked)
    {
        if (locked)
        {
            wanted = CursorLockMode.Locked;
        } else
        {
            wanted = CursorLockMode.None;
        }

        visible = !locked;
        //SetCursor();
    }
}
