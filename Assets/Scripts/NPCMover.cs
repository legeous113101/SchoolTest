using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    float speed = 10f;
    Transform targetTrans;

    private void Start()
    {
        targetTrans = GameObject.Find("Sphere").transform;        
    }
    void MoveTo(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < 0.5f) return;


        transform.forward = (destination - transform.position).normalized;
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void Update()
    {
        MoveTo(targetTrans.position);
    }
}

