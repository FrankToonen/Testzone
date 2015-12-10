using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GM_Ball : NetworkBehaviour
{
    Vector3 startPosition;
    public string lastHitBy;
    RawImage indicator;

    [SerializeField]
    GameObject
        explosion;

    void Start()
    {
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
        RpcPlayExplosion();
        RpcSetVisible(false);

        yield return new WaitForSeconds(3);

        RpcSetVisible(true);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = startPosition;
    }

    [ClientRpc]
    void RpcSetVisible(bool visible)
    {
        GetComponent<SphereCollider>().enabled = visible;
        GetComponent<MeshRenderer>().enabled = visible;
        indicator.enabled = visible;
        if (!visible)
        {
            GetComponent<ParticleSystem>().Stop();
        } else
        {
            GetComponent<ParticleSystem>().Play();
        }

        lastHitBy = "";
    }

    [ClientRpc]
    void RpcPlayExplosion()
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

        explosion.GetComponent<ParticleSystem>().startColor = explosionColor;
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
