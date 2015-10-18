using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Terraformer : Gun
{
    public float radius;
    Camera myCamera;

    void Start()
    {
        myCamera = GetComponentInChildren<Camera>();
        GetComponent<TestPlayer>().EventShoot += Shoot;
    }

    public void Shoot(Vector3 point, int dir)
    {
        Collider[] objectsHit = Physics.OverlapSphere(point, radius);
        for (int n = 0; n < objectsHit.Length; n++)
        {
            Hexagon hex = objectsHit [n].gameObject.GetComponent<Hexagon>();
            if (hex == null)
                continue;
					
            float distance = (radius + 0.5f) - Vector3.Distance(hex.transform.position, point);
            distance = distance > 0 ? Mathf.Pow(distance, 2) : 0;
            Vector3 target = Vector3.up * distance * dir;
					
            if (distance > 0)
            {
                hex.StopAllCoroutines();
                hex.StartCoroutine("MoveTo", target);
                hex.ChangeColor(Color.red);
            }
        }
    }

    public Vector3 ShootRayCast()
    {
        RaycastHit hit;
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        { 
            return hit.point;
        }

        return Vector3.zero;
    }

    /*public void Shoot()
    {
        Debug.Log(transform.tag + "Shoot");
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //
            Debug.Log(transform.tag + " Getmouse");
            //

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Vanuit de speler
            if (Physics.Raycast(ray, out hit))
            {
                //DEBUG
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue, 10f);
                Debug.Log(transform.tag + " Raycasthit");
                //


                Vector3 point = hit.transform.position;
                Collider[] objectsHit = Physics.OverlapSphere(point, radius);
                for (int n = 0; n < objectsHit.Length; n++)
                {
                    //
                    Debug.Log(transform.tag + " objectshit");
                    //

                    Hexagon hex = objectsHit [n].gameObject.GetComponent<Hexagon>();
                    if (hex == null)
                        continue;
                    
                    float distance = radius - Vector3.Distance(hex.transform.position, point);
                    distance = distance > 0 ? Mathf.Pow(distance, 2) : 0;
                    Vector3 target = Vector3.up * distance;
                    if (Input.GetMouseButtonDown(1))
                        target *= -1;
                    
                    if (distance > 0)
                    {
                        hex.StopAllCoroutines();
                        
                        hex.StartCoroutine("MoveTo", target);
                        if (Input.GetMouseButtonDown(0))
                            hex.RpcChangeColor(Color.red);
                        else
                            hex.RpcChangeColor(Color.green);
                    }
                }
            }
        }
    }*/
}
