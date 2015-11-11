using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// Bug:
// Als iemand joined en de vlag is opgepakt, kan hij m afpakken
// Waarschijnlijk niet relevant

public class GM_Flag : GM_GameMode
{
    public delegate void FlagHolderChange(string obj);
    [SyncEvent]
    public event FlagHolderChange
        EventChangeFlagHolder;

    [SerializeField]
    BoxCollider
        childCollider;
    Rigidbody rigidBody;

    Vector3 startPosition;
    bool isHeld, onCoolDown;
    float coolDownTime;

    protected override void Start()
    {
        base.Start();

        rigidBody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        isHeld = false;
        coolDownTime = 1;

        EventChangeFlagHolder += ChangeFlagHolder;
    }
	
    void Update()
    {
        if (!isServer)
            return;

        // TEMP
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
            transform.localPosition = new Vector3(0, .5f, -.3f);
            transform.localRotation = Quaternion.AngleAxis(-10, Vector3.up);
                    
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

    public void ResetPosition()
    {
        transform.position = startPosition;
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
