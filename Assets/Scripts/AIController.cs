using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform fooTarget;

    [SerializeField]
    AIData aIData = new AIData();

    private void Start()
    {
        aIData.m_Go = gameObject;

    }

    private void Update()
    {
        if (fooTarget == null) return;
        SteeringBehavior.Seek(aIData, fooTarget.position);
        SteeringBehavior.Move(aIData);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.up + transform.position,Vector3.up + transform.position + transform.forward * 2f);
    }
}
