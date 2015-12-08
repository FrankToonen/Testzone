using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Ball : GM_GameMode
{
    Vector3 startPosition;
    public string lastHitBy;
    RawImage indicator;
    ParticleSystem explosionParticles;

    protected override void Start()
    {
        base.Start();

        explosionParticles = transform.FindChild("Explosion Particles").GetComponent<ParticleSystem>();

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

        PlayExplosion();

        yield return new WaitForSeconds(3);

        explosionParticles.Stop();

        lastHitBy = "";
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<ParticleSystem>().Play();
        indicator.enabled = true;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = startPosition;
    }

    void PlayExplosion()
    {
        Color explosionColor = Color.white;

        GameObject player = GameObject.Find(lastHitBy);
        if (player != null)
        {
            switch (player.GetComponent<Player_Setup>().playerNumber)
            {
                case 0:
                    {
                        explosionColor = Color.red;
                        break;
                    }
                case 1:
                    {
                        explosionColor = Color.green;                    
                        break;
                    }
                case 2:
                    {
                        explosionColor = Color.blue;                    
                        break;
                    }
                case 3:
                    {
                        explosionColor = Color.yellow;                    
                        break;
                    }
            }
        }

        explosionParticles.startColor = explosionColor;
        explosionParticles.Play();
    }
}
