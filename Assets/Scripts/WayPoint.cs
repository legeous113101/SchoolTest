using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField]
    List<WayPoint> neigbors = new List<WayPoint>();
    public WayPoint parent;
    public float costFromStart;
    public float costToTarget;
    public float TotalCost => costFromStart + costToTarget;
    public List<WayPoint> Neigbors
    {
        get => neigbors;
        set
        {
            if (value.Count == 0) return;
            neigbors = value;
        }
    }
    public WayPoint(Vector3 pos)
    {
        transform.position = pos;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        Gizmos.color = Color.green;
        foreach (var item in neigbors)
        {
            Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }

    public bool HasObstacle
    {
        get
        {
            if (parent == null) return false;
            return AstarPathFinder.HasObstacleBetween(parent.transform.position, transform.position);
        }
    } 
}
