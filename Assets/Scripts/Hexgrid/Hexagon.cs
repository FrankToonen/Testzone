using UnityEngine;
using System.Collections;
//using UnityEngine.Networking;

public class Hexagon : MonoBehaviour
{
    public float resetTime;
    Vector3 startPosition;
    float maxHeight, minHeight, moveSpeed;
    public int xValue, zValue;
    bool shouldLaunch;

    public void Initialize(int x, int z, Vector3 pos)
    {
        xValue = x;
        zValue = z;

        SetPositions(pos, true);
        shouldLaunch = false;
        moveSpeed = 15;
    }

    public void SetPositions(Vector3 pos, bool init = false)
    {
        startPosition = pos;
        maxHeight = pos.y + 8;
        minHeight = pos.y - 8;

        if (!init)
        {
            float newY = startPosition.y - transform.position.y;
            StartCoroutine("ResetY", Vector3.up * newY);
        }
    }

    void Update()
    {
        if (!shouldLaunch)
        {
            return;
        }
        
        LaunchPlayer();
        
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (!shouldLaunch)
        {
            return;
        } 
        
        LaunchObject(collision);
    }

    public void MoveHexagon(Vector3 point, float radius, int dir)
    {
        Vector2 hexXZ = new Vector2(transform.position.x, transform.position.z);
        Vector2 pointXZ = new Vector2(point.x, point.z);
            
        float distance = (radius + 0.5f) - Vector2.Distance(hexXZ, pointXZ);
        distance = distance > 0 ? Mathf.Pow(distance, 2) : 0;
            
        if (distance > 0)
        {
            shouldLaunch = true;
            Vector3 target = Vector3.up * distance * dir;
                
            StopAllCoroutines();
            StartCoroutine("MoveTo", target);

            transform.parent.GetComponent<HexChunk>().StopAllCoroutines();
            transform.parent.GetComponent<HexChunk>().StartCoroutine("SplitChunk", 10);
        }
    }

    void LaunchPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float xDif = Mathf.Abs(transform.position.x - player.transform.position.x);
            float zDif = Mathf.Abs(transform.position.z - player.transform.position.z);

            if (xDif < 1 & zDif < 1)
            {
                player.GetComponent<Player_Force>().AddImpact(Vector3.up, 100);
                shouldLaunch = false;
            }
        }
    }

    void LaunchObject(Collision collision)
    {
        Rigidbody rB = collision.collider.attachedRigidbody;
        if (rB != null)
        {
            rB.AddForce(Vector3.up * 50);
            shouldLaunch = false;
            return;
        }
    }

    public IEnumerator MoveTo(Vector3 target)
    {
        Vector3 targetPosition = transform.position + target;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);

        while (Mathf.Abs(transform.position.y - targetPosition.y) > 0.05f && transform.position.y <= maxHeight && transform.position.y >= minHeight)
        {
            float newY = transform.position.y;

            int dir = targetPosition.y > startPosition.y ? 1 : -1;
            float min = startPosition.y > targetPosition.y ? targetPosition.y : transform.position.y; // : startPosition.y
            float max = startPosition.y <= targetPosition.y ? targetPosition.y : transform.position.y; // : startPosition.y
            newY = Mathf.Clamp(newY + /*target.y*/ dir * moveSpeed * Time.deltaTime, min, max);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        transform.position = targetPosition;
        shouldLaunch = false;

        yield return new WaitForSeconds(resetTime);

        StartCoroutine("ResetY", startPosition - transform.position);
    }

    IEnumerator ResetY(Vector3 target)
    {
        while (Mathf.Abs(transform.position.y - startPosition.y) > 0.05f)
        {
            float newY = transform.position.y;

            int dir = target.y > 0 ? 1 : -1;
            float min = startPosition.y > transform.position.y ? transform.position.y : startPosition.y;
            float max = startPosition.y <= transform.position.y ? transform.position.y : startPosition.y;
            newY = Mathf.Clamp(newY + dir * moveSpeed * Time.deltaTime, min, max);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        transform.position = startPosition;
    }
}
