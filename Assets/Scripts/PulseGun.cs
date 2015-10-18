using UnityEngine;
using System.Collections;

public class PulseGun : Gun
{
    //public GameObject player; // Niet nodig wanneer hij een child is van de player
    public float force;

    // Use this for initialization
    void Start()
    {
    }

    /*
    public override void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Vanuit de speler
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Cube")
            {
                //DEBUG
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan, 10f);
                //

                Rigidbody rigidBody = hit.transform.gameObject.GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    hit.transform.position += new Vector3(0, 0.5f, 0);
                    Vector3 direction = Vector3.Normalize(transform.parent.position - hit.transform.position) * -force;
                    if (Input.GetMouseButtonDown(1))
                    {
                        direction = Vector3.Normalize(transform.parent.position - hit.transform.position) * (force / 2);
                        direction.y *= -1;
                    }

                    rigidBody.AddForce(direction);
                }
            }
        }
    }*/
}
