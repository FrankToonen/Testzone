using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player_SyncPosition : NetworkBehaviour
{
    [SyncVar (hook = "SyncPlayerPosition")]
    Vector3
        syncPlayerPos;

    [SyncVar (hook = "SyncCamPosition")]
    Vector3
        syncCamPos;

    [SerializeField]
    Transform
        playerTransform;

    [SerializeField]
    Transform
        camTransform;

    [SerializeField]
    bool
        useHistoricalLerping;

    List<Vector3> syncPosList = new List<Vector3>();

    Vector3 lastPlayerPos, lastCamPos;
    float threshold = 0.1f;
    float closeEnough = 0.1f;
    int lerpRate;
    int normalLerpRate = 25;
    int fasterLerpRate = 35;

    void Start()
    {
        lerpRate = normalLerpRate;
    }

    void Update()
    {
        LerpPosition();
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

            CameraLerping();
        }
    }

    void HistoricalLerping()
    {
        if (syncPosList.Count > 0)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, syncPosList [0], Time.deltaTime * lerpRate);
            
            if (Vector3.Distance(playerTransform.position, syncPosList [0]) < closeEnough)
                syncPosList.RemoveAt(0);
                        
            lerpRate = syncPosList.Count > 10 ? fasterLerpRate : normalLerpRate;
        }
    }
    
    void NormalLerping()
    {
        playerTransform.position = Vector3.Lerp(playerTransform.position, syncPlayerPos, Time.deltaTime * lerpRate);
    }

    void CameraLerping()
    {
        camTransform.position = Vector3.Lerp(camTransform.position, syncCamPos, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 playerPos, Vector3 camPos)
    {
        syncPlayerPos = playerPos;
        syncCamPos = camPos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            if (Vector3.Distance(playerTransform.position, lastPlayerPos) > threshold || Vector3.Distance(camTransform.position, lastCamPos) > threshold)
            {
                CmdProvidePositionToServer(playerTransform.position, camTransform.position);
                lastPlayerPos = playerTransform.position;
                lastCamPos = camTransform.position;
            }
        }
    }

    [Client]
    void SyncPlayerPosition(Vector3 latestPos)
    {
        syncPlayerPos = latestPos;
        if (useHistoricalLerping && !isLocalPlayer)
        {
            syncPosList.Add(syncPlayerPos);
            Debug.Log(transform.name + " " + syncPosList.Count);
        }
    }

    [Client]
    void SyncCamPosition(Vector3 latestPos)
    {
        syncCamPos = latestPos;
    }
}
