using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
    bool isPositive;
    //Lifetime?

    // Use this for initialization
    void Start()
    {
        // Start timer?
    }

    public void Initialize(Transform parent, bool isPositive)
    {
        this.isPositive = isPositive;
        if (isPositive)
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/MagnetPositive");
        else
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/MagnetNegative");

        transform.parent = parent;
    }
	
    // Update is called once per frame
    void Update()
    {
        FindOtherMagnets();
    }

    void FindOtherMagnets()
    {
        GameObject[] magnetsInLevel = GameObject.FindGameObjectsWithTag("Magnet");
        if (magnetsInLevel.Length > 1)
        {
            foreach (GameObject m in magnetsInLevel)
            {
                if (m.transform == transform)
                    continue;

                if (Vector3.Distance(m.transform.position, transform.position) < 5)
                {
                    Vector3 dir = Vector3.Normalize(m.transform.position - transform.position);
                    if (m.GetComponent<Magnet>().isPositive == isPositive)
                        dir *= -1;

                    MoveParent(dir * 1000);

                    //Destroy(this.gameObject);
                    //Destroy(m.gameObject);
                }
            }
        }               
    }

    void MoveParent(Vector3 force)
    {
        if (transform.parent.tag == "Player")
        {
            transform.parent.GetComponent<Player_Force>().AddImpact(force, force.magnitude);
        } else
        {
            Rigidbody parentRigidBody = GetComponentInParent<Rigidbody>();
            if (parentRigidBody != null)
            {
                parentRigidBody.AddForce(force);
            }
        }
    }

    public bool IsPositive
    {
        get { return isPositive; }
    }
}
