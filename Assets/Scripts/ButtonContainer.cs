using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonContainer : MonoBehaviour
{
    public List<GameObject> buttons;

    void FindButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buttons.Add(transform.GetChild(i).gameObject);
            ButtonScript bScript = buttons [i].GetComponent<ButtonScript>();
            if (bScript != null)
            {
                bScript.Initialize();
            }
        }
    }

    public GameObject FindButton(string bName)
    {
        if (buttons.Count == 0)
        {
            FindButtons();
        }

        return buttons.Find(found => found.name == bName);
    }
}
