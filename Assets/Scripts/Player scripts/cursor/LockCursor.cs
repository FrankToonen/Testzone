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

    void SetCursor()
    {
        Cursor.lockState = wanted;
        Cursor.visible = visible;
    }

    void Update()
    {
        //SetCursor();
    }

    public void SetLockState(bool locked)
    {
        if (locked)
        {
            LockMouse();
        } else
        {
            UnlockMouse();
        }

        visible = !locked;
        SetCursor();
    }

    void LockMouse()
    {
        wanted = CursorLockMode.Locked;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
