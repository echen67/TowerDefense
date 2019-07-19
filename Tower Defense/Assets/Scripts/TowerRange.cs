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
                Vector3 enemyVelocity = firstEnemy.GetComponent<EnemyMovement>().getVelocity();
                Vector3 target = ShootEnemy(firstEnemy.transform.position, enemyVelocity, transform.position, shootSpeed);
                Vector3 force = target - arrowInstance.transform.position;
                arrowInstance.transform.rotation = Quaternion.LookRotation(Vector3.forward, force);
                force = force.normalized * shootSpeed;
                arrowInstance.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            }
        }
        timer++;
    }

    //https://forum.unity.com/threads/projectile-trajectory-accounting-for-gravity-velocity-mass-distance.425560/
    // use predictive aiming to shoot enemy
    private Vector3 ShootEnemy(Vector3 enemyPosition, Vector3 enemyVelocity, Vector3 towerPosition, float arrowSpeed)
    {
        Vector3 enemyRelPos = enemyPosition - towerPosition;
        Vector3 enemyRelVel = enemyVelocity;
        float t = FirstInterceptTime(arrowSpeed, enemyRelPos, enemyRelVel);
        return enemyPosition + t * (enemyRelVel);
    }

    private float FirstInterceptTime(float arrowSpeed, Vector3 enemyRelPos, Vector3 enemyRelVel)
    {
        float velocitySquared = enemyRelVel.sqrMagnitude;
        if (velocitySquared < 0.001f)
        {
            return 0f;
        }
        float a = velocitySquared - arrowSpeed * arrowSpeed;

        // handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -enemyRelPos.sqrMagnitude / (2f * Vector3.Dot(enemyRelVel, enemyRelPos));
            return Mathf.Max(t, 0f);    //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(enemyRelVel, enemyRelPos);
        float c = enemyRelPos.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        {
            //determinant > 0; two intercept paths
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a);
            float t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2);   //both are positive
                else
                    return t1;  //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f);   //don't shoot back in time
        } else if (determinant < 0f)    //no intercept path
        {
            return 0f;
        } else
        {
            // determinant = 0; one intercept path
            return Mathf.Max(-b / (2f * a), 0f);    //don't shoot back in time
        }
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
