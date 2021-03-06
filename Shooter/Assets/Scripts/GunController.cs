﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EZEffects;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GunController : MonoBehaviour
{
    public Transform Barrel;
    public Transform Trigger;
    public GameObject BulletHolePrefab;
    public float Range = 100f;
    private Stopwatch stopWatch;

    private int hits = 0;
    private int missed = 0;

    public readonly int Damage = 10;

    private List<GameObject> m_bulletHoles = null;
    private int m_ammo = 0;
    private readonly int m_magazineSize = 5;
    public LineRenderer GunLine;
    private Ray RayFromGun;

    private Vector3 currentPosition;
    private Vector3 pointOn5;
    private Vector3 pointOn10;
    private Vector3 pointOn15;

    public bool ShowRay;

    private void Awake()
    {
        m_ammo = m_magazineSize;

        m_bulletHoles = CreateBulletHoles(BulletHolePrefab, m_magazineSize);
        GunLine = GetComponent<LineRenderer>();
        DisableEffects();
        ShowRay = false;
        stopWatch = new Stopwatch();
    }

    private void Update()
    {
        RayFromGun = new Ray(Barrel.position, Barrel.forward);
        DisplayRay();

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

    public void StartStopWatch()
    {
        stopWatch.Reset();
        currentPosition = RayFromGun.origin;

        pointOn5 = RayFromGun.GetPoint(5);
        pointOn10 = RayFromGun.GetPoint(10);
        pointOn15 = RayFromGun.GetPoint(15);

        stopWatch.Start();
    }

    public void StopStopWatch()
    {
        stopWatch.Stop();
        Debug.Log("Time spent for shot: " + stopWatch.ElapsedMilliseconds);
    }

    public void Fire()
    {
        m_ammo--;

        if (m_ammo < 0)
        {
            m_ammo = m_magazineSize;
        }


        RaycastHit hit;

        if (Physics.Raycast(RayFromGun, out hit))
        {
            var gameObject = hit.collider.gameObject;

            if (gameObject.CompareTag("Target"))
            {
                StopStopWatch();
                hits++;
                Debug.Log("Hits: " + hits);
                float distance = -1;
                var positionZ = gameObject.transform.position.z;

                if (positionZ.Equals(5))
                {
                    distance = Vector3.Distance(pointOn5, gameObject.transform.position);
                    Debug.Log("Distance at 5: " + distance);
                }
                else if (positionZ.Equals(10))
                {
                    distance = Vector3.Distance(pointOn10, gameObject.transform.position);
                    Debug.Log("Distance at 10: " + distance);
                }
                else if (positionZ.Equals(15))
                {
                    distance = Vector3.Distance(pointOn15, gameObject.transform.position);
                    Debug.Log("Distance at 15: " + distance);
                }

            }
            else
            {
                missed++;
                Debug.Log("Missed: " + missed);
            }
            CheckForDamage(hit.collider.gameObject);

            PlaceBulletHole(m_bulletHoles[m_ammo], hit);
        }
    }

    private void CheckForDamage(GameObject hitObject)
    {
        if (!hitObject.CompareTag("Target")) { return; }

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
