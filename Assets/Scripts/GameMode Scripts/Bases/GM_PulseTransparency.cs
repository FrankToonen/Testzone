using UnityEngine;
using System.Collections;

public class GM_PulseTransparency : MonoBehaviour
{
    float timeActive, transparency;
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        timeActive += Time.deltaTime;
        //transparency = Mathf.Clamp(Mathf.Abs(Mathf.Sin(timeActive)), 0.25f, 0.75f);
        transparency = (Mathf.Abs(Mathf.Sin(timeActive)) / 2) + 0.35f;
        
        Color color = renderer.material.color;
        color.a = transparency;
        renderer.material.color = color;
    }
}
