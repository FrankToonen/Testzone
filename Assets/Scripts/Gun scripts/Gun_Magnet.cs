using UnityEngine;
using System.Collections;

public class Gun_Magnet : MonoBehaviour
{
    bool isPositive;
    [SerializeField]
    float
        lifeTime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Timer(lifeTime));
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

                Gun_Magnet magnet = m.GetComponent<Gun_Magnet>();

                if (Vector3.Distance(magnet.transform.position, transform.position) < 5)
                {
                    Vector3 dir = Vector3.Normalize(magnet.transform.position - transform.position);
                    if (magnet.GetComponent<Gun_Magnet>().isPositive == isPositive)
                        dir *= -1;

                    MoveParent(dir * 1000);
                    magnet.MoveParent(-dir * 1000);

                    Destroy(this.gameObject);
                    Destroy(magnet.gameObject);
                }
            }
        }               
    }

    public void MoveParent(Vector3 force)
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

    IEnumerator Timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
