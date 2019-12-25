using System.Collections;
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
    Vector3 velocity;

    private int value = 10;
    private Money moneyScript;
    private Health healthScript;

    public Vector3 getVelocity()
    {
        return velocity;
    }

    public int getWaypointIndex()
    {
        return waypointIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrow")
        {
            Destroy(collision.gameObject);
            moneyScript.addMoney(value);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        waypoints = WaypointManager.waypoints;
        moneyScript = GameObject.FindGameObjectWithTag("Money").GetComponent<Money>();
        healthScript = GameObject.FindGameObjectWithTag("Health").GetComponent<Health>();
    }

    void Update()
    {
        // when enemy has reached the last waypoint (the end)
        if (transform.position == waypoints[waypointIndex].position)
        {
            // decrease health
            healthScript.loseHealth(1);
            // destroy (or set to inactive if using object pooling)
            Destroy(gameObject);
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, Time.deltaTime * moveSpeed);
        // when enemy has reached the target waypoint, increment waypoint index
        if (transform.position == waypoints[waypointIndex].position && waypointIndex != waypoints.Length-1)
        {
            waypointIndex++;
        }

        velocity = waypoints[waypointIndex].position - transform.position;
        velocity.Normalize();
        velocity *= moveSpeed;
    }
}
