using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarAI : MonoBehaviour
{
    [SerializeField]
    AIData aIData;

    Transform currentTrans;

    List<Vector3> path = new List<Vector3>();
    void OnMouseClick()
    {
        currentTrans = transform;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == 9) // hit the ground
            {
                InitPath(hit.point);
            }
        }


    }
    void InitPath(Vector3 goal)
    {
        path.Clear();
        //var rawPath = new List<Vector3>();
        var isGoalInsight = !AstarPathFinder.HasObstacleBetween(transform.position, goal);
        if (isGoalInsight)
        {
            path.Add(transform.position);
            path.Add(goal);
            return;
        }
        var startWayPoint = InitWayPoint(transform.position);
        var goalWayPoint = SearchNearestWayPoint(goal);


        var tempPath = AstarPathFinder.Search(startWayPoint, goalWayPoint);
        DestroyTempWayPoint();
        //path.Add(currentTrans.position);
        foreach (var pos in tempPath)
        {
            path.Add(pos.transform.position);
        }

    }

    WayPoint SearchNearestWayPoint(Vector3 pivot)
    {
        var wayPoints = FindObjectsOfType<WayPoint>();
        if (wayPoints == null) return null;
        WayPoint rt = wayPoints[0];
        foreach (var wp in wayPoints)
        {
            bool isNearer = Vector3.Distance(pivot, wp.transform.position) < Vector3.Distance(pivot, rt.transform.position);
            if (isNearer)
            {
                Ray ray = new Ray(pivot, wp.transform.position - pivot);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == 8) continue; // has obstacle between.
                }
                rt = wp;
            }
        }
        return rt;
    }

    WayPoint InitWayPoint(Vector3 pos)
    {
        var allWayPoints = FindObjectsOfType<WayPoint>();
        GameObject wayGo = new GameObject("tempWayPoint");
        //wayGo = Instantiate(wayGo, pos, new Quaternion());
        wayGo.AddComponent<WayPoint>();
        wayGo.transform.position = pos;//
        List<WayPoint> neihgbors = new List<WayPoint>();
        for (int i = 0; i < allWayPoints.Length; i++)
        {
            if (!AstarPathFinder.HasObstacleBetween(wayGo.transform.position, allWayPoints[i].transform.position))
            {
                neihgbors.Add(allWayPoints[i]);
            }
        }
        wayGo.GetComponent<WayPoint>().Neigbors = neihgbors;
        return wayGo.GetComponent<WayPoint>();
    }

    void DestroyTempWayPoint()
    {
        Destroy(GameObject.Find("tempWayPoint"));
    }



    private void OnDrawGizmos()
    {
        if (path == null) return;
        if (path.Count > 0)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick();
        }
    }
}
