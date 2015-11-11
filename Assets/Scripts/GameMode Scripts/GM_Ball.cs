using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GM_Ball : GM_GameMode
{
    Vector3 startPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public void Reset()
    {
        if (!manager.RoundFinished)
        {
            return;
        }

        transform.position = startPosition;
    }
}
