﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    /// <summary>
    /// Attached to enemies
    /// Moves from waypoint to waypoint
    /// </summary>

    Transform[] waypoints;
    int waypointIndex = 0;
    float moveSpeed = 2f;

    public int getWaypointIndex()
    {
        return waypointIndex;
    }

    void Start()
    {
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
        if (transform.position == waypoints[waypointIndex].position && waypointIndex != waypoints.Length-1)
        {
            waypointIndex++;
        }
    }
}
