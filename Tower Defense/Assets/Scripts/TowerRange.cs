using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class TowerRange : MonoBehaviour
{
    /// <summary>
    /// Handles the tower's shooting
    /// Currently targets the first enemy within range
    /// </summary>

    public GameObject arrow;

    public List<GameObject> enemies;
    public bool enableShoot = false;
    public int shootTime = 50;
    public int timer = 0;

    private int shootSpeed = 5;
    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        DrawCircle(2);
    }

    // Shoot every so often
    void Update()
    {
        if (enableShoot && timer >= shootTime)
        {
            timer = 0;
            GameObject firstEnemy = FindFirstEnemy();
            if (firstEnemy != null)
            {
                GameObject arrowInstance = Instantiate(arrow, transform.parent.transform.position, Quaternion.identity);
                Vector3 force = firstEnemy.transform.position - arrowInstance.transform.position;
                arrowInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward, force);
                force = force.normalized * shootSpeed;
                arrowInstance.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            }
        }
        timer++;
    }

    // Find the first enemy within this tower's range
    public GameObject FindFirstEnemy()
    {
        if (enemies == null || enemies.Count == 0)
        {
            return null;
        }
        GameObject first = enemies[0];
        for (int i = 0; i < enemies.Count; i++)
        {
            if (first == null)
            {
                first = enemies[i];
            } else
            {
                int curWaypoint = enemies[i].GetComponent<EnemyMovement>().getWaypointIndex();
                int firstWaypoint = first.GetComponent<EnemyMovement>().getWaypointIndex();
                if (curWaypoint > firstWaypoint)
                {
                    first = enemies[i];
                } else if (curWaypoint == firstWaypoint)
                {
                    Transform target = WaypointManager.waypoints[curWaypoint];  // the waypoint enemy is heading for
                    float firstDist = Vector2.Distance(target.position, first.transform.position);
                    float curDist = Vector2.Distance(target.position, enemies[i].transform.position);
                    if (curDist < firstDist)
                    {
                        first = enemies[i];
                    }
                }
            }
        }
        return first;
    }

    // When an enemy enters the range, add it to the list
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && enableShoot)
        {
            if (enemies == null || enemies.Count == 0)
            {
                timer = shootTime;
            }
            enemies.Add(collision.gameObject);
        }
    }

    // When an enemy leaves the range, remove it from the list
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && enableShoot)
        {
            enemies.Remove(collision.gameObject);
        }
    }

    private void DrawCircle(float radius)
    {
        float linewidth = 0.05f;
        int segments = 32;
        line.useWorldSpace = false;
        line.positionCount = segments + 1;
        line.startWidth = linewidth;
        line.endWidth = linewidth;
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i < segments + 1; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
        }
        line.SetPositions(points);
    }
}
