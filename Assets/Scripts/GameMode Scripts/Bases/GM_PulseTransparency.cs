using UnityEngine;
using System.Collections;

public class GM_PulseTransparency : MonoBehaviour
{
    float timeActive, transparency;
    bool fadingOut;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        timeActive += Time.deltaTime;

        if (timeActive < 30)
        {
            transparency = (Mathf.Abs(Mathf.Sin(timeActive)) / 2) + 0.35f;
        } else if (timeActive >= 30 && !fadingOut)
        {
            FadeOut();
        } else if (fadingOut)
        {
            transparency = 1 - ((timeActive - 30) / 30);
        }

        Color color = rend.material.color;
        color.a = transparency;
        rend.material.color = color;
    }

    void OnEnable()
    {
        timeActive = 0;
        fadingOut = false;
    }

    void FadeOut()
    {
        fadingOut = true;
    }
}
