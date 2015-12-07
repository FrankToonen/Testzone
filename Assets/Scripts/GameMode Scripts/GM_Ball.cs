using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Ball : GM_GameMode
{
    Vector3 startPosition;
    public string lastHitBy;
    RawImage indicator;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        transform.name = "Ball";
        indicator = GameObject.Find("Objective Indicator").GetComponent<RawImage>();
    }

    public void ResetPosition()
    {
        StartCoroutine(ResetBall());

    }

    IEnumerator ResetBall()
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Stop();
        indicator.enabled = false;

        yield return new WaitForSeconds(3);

        lastHitBy = "";
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<ParticleSystem>().Play();
        indicator.enabled = true;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = startPosition;
    }
}
