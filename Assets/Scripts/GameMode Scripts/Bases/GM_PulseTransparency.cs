using UnityEngine;
using System.Collections;

public class GM_PulseTransparency : MonoBehaviour
{
    float timeActive, transparency;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        timeActive += Time.deltaTime;
        transparency = (Mathf.Abs(Mathf.Sin(timeActive)) / 2) + 0.35f;
        
        Color color = rend.material.color;
        color.a = transparency;
        rend.material.color = color;
    }
}
