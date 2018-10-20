using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    Transform[] wayPoints;
    [SerializeField]
    bool changingVelocity = false;
    int currentWayPoint = 0;
    Rigidbody rigidB;
    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (changingVelocity) { randomizeVelocity(); }
        Movement();
    }

    void Movement()
    {
        if (Vector3.Distance(transform.position, wayPoints[currentWayPoint].position) < 0.75f)
        {
            currentWayPoint += 1;
            currentWayPoint = currentWayPoint % wayPoints.Length;
        }

        Vector3 _dir = (wayPoints[currentWayPoint].position - transform.position).normalized;
        rigidB.MovePosition(transform.position + _dir * moveSpeed * Time.deltaTime);
    }

    void randomizeVelocity() {
        moveSpeed = Random.Range(1, 40);
    }
}
