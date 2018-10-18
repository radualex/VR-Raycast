﻿using System.Collections;
using System.Collections.Generic;
using EZEffects;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform Barrel;
    public Transform Trigger;
    public GameObject BulletHolePrefab;
    public float Range = 100f;



    public readonly int Damage = 10;

    private List<GameObject> m_bulletHoles = null;
    private int m_ammo = 0;
    private readonly int m_magazineSize = 5;
    public LineRenderer GunLine;
    private Ray RayFromGun;
    public bool ShowRay;

    private void Awake()
    {
        m_ammo = m_magazineSize;

        m_bulletHoles = CreateBulletHoles(BulletHolePrefab, m_magazineSize);
        GunLine = GetComponent<LineRenderer>();
        DisableEffects();
        ShowRay = false;
    }

    private void Update()
    {
        RayFromGun = new Ray(Barrel.position, Barrel.forward);
        if (ShowRay)
        {
            DisplayRay();
        }
        else
        {
            DisableEffects();
        }
    }

    private void DisableEffects()
    {
        GunLine.enabled = false;
    }
    public void SetTriggerRotation(float triggerValue)
    {
        var targetX = triggerValue * 25.0f;
        Trigger.transform.localEulerAngles = new Vector3(targetX, 0, 0);
    }

    private void DisplayRay()
    {
        GunLine.enabled = true;
        GunLine.SetPosition(0, transform.position);

        GunLine.SetPosition(1, RayFromGun.origin + RayFromGun.direction * Range);
    }

    public void Fire()
    {
        if (m_ammo < 0)
        {
            m_ammo = m_magazineSize;
        }
        m_ammo--;

        RaycastHit hit;

        if (Physics.Raycast(RayFromGun, out hit))
        {
            CheckForDamage(hit.collider.gameObject);

            PlaceBulletHole(m_bulletHoles[m_ammo - 1], hit);
        }
    }

    private void CheckForDamage(GameObject hitObject)
    {
        if(!hitObject.CompareTag("Target")) { return; }

        var target = hitObject.GetComponent<Target>();
        target.Damage(this);
    }

    private List<GameObject> CreateBulletHoles(GameObject bulletHole, int magazineSize)
    {
        var newBulletHoles = new List<GameObject>();

        for (var i = 0; i < magazineSize; i++)
        {
            var newBulletHole = Instantiate(bulletHole);
            newBulletHole.SetActive(false);

            newBulletHoles.Add(newBulletHole);
        }

        return newBulletHoles;
    }

    private void PlaceBulletHole(GameObject bulletHole, RaycastHit hit)
    {
        //Orient
        bulletHole.transform.position = hit.point;
        bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.back, hit.normal);

        //Enable
        bulletHole.transform.parent = hit.transform;
        bulletHole.SetActive(true);
    }
}
