using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Player_SyncRotation : NetworkBehaviour
{
    [SyncVar (hook = "OnPlayerRotSynced")]
    float
        syncPlayerRotation;

    [SyncVar (hook = "OnCamRotSynced")]
    float
        syncCamRotation;

    [SerializeField]
    Transform
        playerTransform;

    [SerializeField]
    Transform
        camTransform;

    List<float> syncPlayerRotList = new List<float>();
    List<float> syncCamRotList = new List<float>();

    int lerpRate = 20;
    float lastPlayerRot, lastCamRot;
    float threshold = 0.3f;
    float CloseEnough = 0.4f;

    [SerializeField]
    bool
        useHistoricalLerping;
	
    void Update()
    {
        LerpRotations();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TransmitRotations();
    }

    void LerpRotations()
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
        if (syncPlayerRotList.Count > 0)
        {
            LerpPlayerRotation(syncPlayerRotList [0]);
            if (Mathf.Abs(playerTransform.localEulerAngles.y - syncPlayerRotList [0]) < CloseEnough)
                syncPlayerRotList.RemoveAt(0);
        }
        
        if (syncCamRotList.Count > 0)
        {
            LerpCamRotation(syncCamRotList [0]);
            if (Mathf.Abs(camTransform.localEulerAngles.x - syncCamRotList [0]) < CloseEnough)
                syncCamRotList.RemoveAt(0);
        }
    }

    void NormalLerping()
    {
        LerpPlayerRotation(syncPlayerRotation);
        LerpCamRotation(syncCamRotation);
    }

    void LerpPlayerRotation(float rotAngle)
    {
        Vector3 playerNewRot = new Vector3(0, rotAngle, 0);
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.Euler(playerNewRot), lerpRate * Time.deltaTime);
    }

    void LerpCamRotation(float rotAngle)
    {
        Vector3 camNewRot = new Vector3(rotAngle, 0, 0);
        camTransform.localRotation = Quaternion.Lerp(camTransform.localRotation, Quaternion.Euler(camNewRot), lerpRate * Time.deltaTime);
    }

    [Command]
    void CmdProvideRotationsToServer(float playerRot, float camRot)
    {
        syncPlayerRotation = playerRot;
        syncCamRotation = camRot;
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer && (CheckThreshold(playerTransform.localEulerAngles.y, lastPlayerRot) || CheckThreshold(camTransform.localEulerAngles.x, lastCamRot)))
        {
            lastPlayerRot = playerTransform.localEulerAngles.y;
            lastCamRot = camTransform.localEulerAngles.x;
            CmdProvideRotationsToServer(lastPlayerRot, lastCamRot);
        }
    }

    [Client]
    void OnPlayerRotSynced(float latestPlayerRot)
    {
        syncPlayerRotation = latestPlayerRot;
        syncPlayerRotList.Add(syncPlayerRotation);
    }

    [Client]
    void OnCamRotSynced(float latestCamRot)
    {
        syncCamRotation = latestCamRot;
        syncCamRotList.Add(syncCamRotation);
    }

    bool CheckThreshold(float rot1, float rot2)
    {
        return Mathf.Abs(rot1 - rot2) > threshold;
    }
}
