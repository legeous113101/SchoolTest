using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPathFinder
{
    static List<WayPoint> open = new List<WayPoint>();
    static List<WayPoint> close = new List<WayPoint>();


    public static List<WayPoint> Search(WayPoint start, WayPoint goal)
    {
        open.Clear();
        close.Clear();
        start.costFromStart = 0f;
        start.costToTarget = Vector3.Distance(start.transform.position, goal.transform.position);
        start.parent = null;
        ////
        //var startNodeCandidates = start.Neigbors;
        //startNodeCandidates.Add(start);
        //startNodeCandidates.ForEach(o => InitWayPointData())
        //
        WayPoint current = start;
        open.Add(current);
        while(open.Count > 0)
        {
            if (current == goal) break;
            var neigbors = current.Neigbors;
            for (int i = 0; i < neigbors.Count; i++)
            {
                var isInOpen = open.Any(o => o == neigbors[i]);
                var isInClose = close.Any(o => o == neigbors[i]);
                if (isInOpen || isInClose)
                {
                    var temp = current.costFromStart + Vector3.Distance(current.transform.position, neigbors[i].transform.position);
                    if (temp < neigbors[i].costFromStart)
                    {
                        neigbors[i] = InitWayPointData(neigbors[i], current, goal);
                        if (neigbors[i].HasObstacle) continue;
                        if (isInClose)
                        {
                            close.Remove(neigbors[i]);
                            open.Add(neigbors[i]);
                        }
                    }
                }
                else
                {
                    neigbors[i] = InitWayPointData(neigbors[i], current, goal);
                    if (neigbors[i].HasObstacle) continue;
                    open.Add(neigbors[i]);
                }
            }
            WayPoint nextPoint = open[0];
            foreach (var point in open)
            {
                if (point == current) continue;
                if (point.TotalCost < nextPoint.TotalCost) nextPoint = point;
            }
            open.Remove(current);
            close.Add(current);
            current = nextPoint;
        }
        if (current != goal) throw new System.Exception("Path Finding Failed");
        return BuidPath(current);
    }


    static WayPoint InitWayPointData(WayPoint point, WayPoint previous, WayPoint goal)
    {
        point.costFromStart = previous.costFromStart + Vector3.Distance(point.transform.position, previous.transform.position);

        //If there are obstacle between point and goal, even if their distance is close, you can't go there.need to fix.
        point.costToTarget = Vector3.Distance(point.transform.position, goal.transform.position);
        point.parent = previous;
        return point;
    }

    static List<WayPoint> BuidPath(WayPoint goal)
    {
        List<WayPoint> rt = new List<WayPoint>();
        WayPoint agent = goal;
        rt.Add(agent);
        while(agent.parent != null)
        {
            var previous = agent.parent;
            if(previous != null)
            {
                rt.Add(previous);
                agent = previous;
            }
        }
        rt.Reverse();
        return rt;        
    }
    public static bool HasObstacleBetween(Vector3 from, Vector3 to)
    {
        Ray ray = new Ray(from, to - from);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject.layer == 8;
        }
        return false;
    }
}
