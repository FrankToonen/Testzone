using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Network_SyncPosition : NetworkBehaviour
{
    [SyncVar (hook = "SyncPositionValues")]
    Vector3
        syncPos;

    [SerializeField]
    Transform
        myTransform;

    [SerializeField]
    bool
        useHistoricalLerping;

    List<Vector3> syncPosList = new List<Vector3>();

    Vector3 lastPos;
    float threshold = .1f;
    float closeEnough = 0.1f;
    int lerpRate;
    int normalLerpRate = 18;
    int fasterLerpRate = 30;
    int latency;

    void Start()
    {
        lerpRate = normalLerpRate;
        NetworkServer.SpawnObjects();
    }

    void Update()
    {
        LerpPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TransmitPosition();
    }

    void LerpPosition()
    {
        if (!isServer)
        {
            if (useHistoricalLerping)
            {
                HistoricalLerping();
            } else
            {
                NormalLerping();
            }
        }
    }

    void HistoricalLerping()
    {
        if (syncPosList.Count > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList [0], Time.deltaTime * lerpRate);
            
            if (Vector3.Distance(myTransform.position, syncPosList [0]) < closeEnough)
                syncPosList.RemoveAt(0);
                        
            lerpRate = syncPosList.Count > 10 ? fasterLerpRate : normalLerpRate;
        }
    }
    
    void NormalLerping()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isServer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }

    [Client]
    void SyncPositionValues(Vector3 latestPos)
    {
        syncPos = latestPos;
        syncPosList.Add(syncPos);
    }
}
