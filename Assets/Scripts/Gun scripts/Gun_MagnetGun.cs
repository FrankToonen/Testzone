﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gun_MagnetGun : Gun
{    
    //[SerializeField]
    Gun_Magnet
        magnetPrefab;

    protected override void Start()
    {
        base.Start();
        magnetPrefab = Resources.Load<Gun_Magnet>("Prefabs/Magnet");
        //soundName = /*"magnetgun_01"*/ "pulsegun_02";
        reloadTime = 1;
    }
    
    protected override void ShootPrimary(string objectHit, Vector3 point, float charge)
    {
        ShootMagnetGun(objectHit, point, true);

    }
    
    protected override void ShootSecondary(string objectHit, Vector3 point, float charge)
    {
        ShootMagnetGun(objectHit, point, false);
    }
    
    void ShootMagnetGun(string objectHit, Vector3 point, bool isPositive)
    {
        GameObject obj = GameObject.Find(objectHit);
        if (obj != null)
        {
            Gun_Magnet newMagnet = Instantiate(magnetPrefab, point, Quaternion.identity) as Gun_Magnet; // Rotation overnemen van geraakt object?
            newMagnet.Initialize(obj.transform, isPositive);
        }
    }
}
