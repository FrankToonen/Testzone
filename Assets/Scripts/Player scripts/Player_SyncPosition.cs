﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player_SyncPosition : NetworkBehaviour
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

    NetworkClient nClient;
    Text latencyText;
    List<Vector3> syncPosList = new List<Vector3>();

    Vector3 lastPos;
    float threshold = 0.1f;
    float closeEnough = 0.1f;
    int lerpRate;
    int normalLerpRate = 30;
    int fasterLerpRate = 45;
    int latency;

    void Start()
    {
        nClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
        latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
        lerpRate = normalLerpRate;
    }

    void Update()
    {
        LerpPosition();
        //ShowLatency();
    }

    void FixedUpdate()
    {
        TransmitPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
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
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }

    [Client]
    void SyncPositionValues(Vector3 latestPos)
    {
        syncPos = latestPos;
        if (useHistoricalLerping)
            syncPosList.Add(syncPos);
    }

    void ShowLatency()
    {
        if (isLocalPlayer)
        {
            latency = nClient.GetRTT();
            latencyText.text = "Latency: " + latency.ToString();
        }
    }
}
