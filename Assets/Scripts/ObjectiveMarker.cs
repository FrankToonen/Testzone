using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ObjectiveMarker : MonoBehaviour
{
    GameObject ball;
    Camera cam;
    bool isVisible;

    void Start()
    {
        GetComponent<RawImage>().enabled = false;
        ;
    }
	
    void Update()
    {
        if (cam == null)
        {
            FindLocalCam();
        } else if (ball == null)
        {
            FindBall();
        } else
        {
            Vector3 screenPos = cam.WorldToScreenPoint(ball.transform.position);

            if (screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height)
            {
                float distance = Vector3.Distance(transform.position, cam.transform.position) / 1250;
                transform.localScale = new Vector3(distance, distance, distance);
                transform.position = screenPos + new Vector3(0, distance * 100, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 180);
            } else
            {
                if (screenPos.z < 0)
                {
                    screenPos *= -1;
                }

                Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

                screenPos -= screenCenter;

                float angle = Mathf.Atan2(screenPos.y, screenPos.x);
                angle -= 90 * Mathf.Deg2Rad;

                float cos = Mathf.Cos(angle);
                float sin = -Mathf.Sin(angle);

                screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

                /// Rand scherm
                /*float m = cos / sin;

                Vector3 screenBounds = screenCenter * 0.9f;

                if (cos > 0)
                {
                    screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0);
                } else
                {
                    screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
                }

                if (screenPos.x > screenBounds.x)
                {
                    screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
                } else if (screenPos.x < -screenBounds.x)
                {
                    screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
                }*/
                ///

                /// Radius om midden scherm
                int radius = 75;
                screenPos = new Vector3(sin * radius, cos * radius, 0);
                ///

                transform.localScale = new Vector3(.3f, .3f, .3f);
                transform.localPosition = screenPos;
                transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }
        }
    }

    void FindBall()
    {
        ball = GameObject.FindWithTag("Ball");
        GetComponent<RawImage>().enabled = ball != null;
    }

    void FindLocalCam()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                cam = p.GetComponentInChildren<Camera>();
            }
        }
    }
}
