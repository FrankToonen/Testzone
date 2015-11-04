using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// Bug:
// Als iemand joined en de vlag is opgepakt, kan hij m afpakken
// Waarschijnlijk niet relevant

public class CTF_Flag : NetworkBehaviour
{
    public delegate void FlagHolderChange(string obj);
    [SyncEvent]
    public event FlagHolderChange
        EventChangeFlagHolder;

    [SerializeField]
    BoxCollider
        childCollider;
    Rigidbody rigidBody;

    bool isHeld, onCoolDown;
    float coolDownTime;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        isHeld = false;
        coolDownTime = 1;

        EventChangeFlagHolder += ChangeFlagHolder;
    }
	
    void Update()
    {
        if (!isServer)
            return;

        if (isHeld)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (transform.parent != null)
                {
                    transform.parent.GetComponent<Player_Force>().AddImpact(Vector3.up, 50);
                    CmdChangeFlagHolder("");
                }
            }
        }
    }

    // When entered, check if it's a player and if so, parent it
    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.transform.tag == "Player" && !isHeld && !onCoolDown)
            {
                CmdChangeFlagHolder(other.transform.name);
            }
        }
    }

    void ChangeFlagHolder(string obj)
    {
        if (obj != "")
        {
            PickUp(obj);
        } else
        {
            KnockOff();
        }
    }

    void PickUp(string n)
    {
        GameObject obj = GameObject.Find(n);
        if (obj != null && !isHeld)
        {
            transform.parent = obj.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
                    
            rigidBody.useGravity = false;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                    
            childCollider.enabled = false;
            isHeld = true;
            GetComponent<Network_SyncPosition>().shouldLerp = false;
        }
    }

    // Removes the flag from the player holding it
    void KnockOff()
    {
        transform.parent = null;
        transform.rotation = Quaternion.identity;

        rigidBody.useGravity = true;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rigidBody.AddForce(-Vector3.up * 5);

        GetComponent<Network_SyncPosition>().shouldLerp = true;

        StartCoroutine(CoolDown(coolDownTime));
    }

    IEnumerator CoolDown(float time)
    {
        onCoolDown = true;

        yield return new WaitForSeconds(.1f);
        childCollider.enabled = true;

        yield return new WaitForSeconds(time);

        onCoolDown = false;
        isHeld = false;

    }
    
    [Command]
    public void CmdChangeFlagHolder(string obj)
    {
        EventChangeFlagHolder(obj);
    }
}
