using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // attached to enemy

    Transform[] waypoints;
    int waypointIndex = 0;
    float moveSpeed = 2f;

    public int getWaypointIndex()
    {
        return waypointIndex;
    }

    void Start()
    {
        //waypoints = GameObject.FindGameObjectWithTag("Waypoint").GetComponentsInChildren<Transform>();
        //Transform waypointManager = GameObject.FindGameObjectWithTag("Waypoint").transform;
        //int numChildren = waypointManager.childCount;
        //waypoints = new Transform[numChildren];
        //for (int i = 0; i < numChildren; i++)
        //{
        //    waypoints[i] = waypointManager.GetChild(i);
        //}
        waypoints = WaypointManager.waypoints;
    }

    void Update()
    {
        // when enemy has reached the last waypoint (the end)
        if (transform.position == waypoints[waypointIndex].position)
        {
            // destroy (or set to inactive if using object pooling)
            Destroy(gameObject);
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, Time.deltaTime * moveSpeed);
        // when enemy has reached the target waypoint, increment waypoint index
        if (transform.position == waypoints[waypointIndex].position)
        {
            if (waypointIndex != waypoints.Length-1)
            {
                waypointIndex++;
            }
        }
    }
}
