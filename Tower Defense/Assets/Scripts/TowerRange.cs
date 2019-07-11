using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class TowerRange : MonoBehaviour
{
    public GameObject arrow;
    public List<GameObject> enemies;
    public bool enableShoot = false;
    public int shootTime = 50;
    public int timer = 0;
    //GameObject first;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enableShoot && timer >= shootTime)
        {
            timer = 0;
            GameObject firstEnemy = FindFirstEnemy();
            if (firstEnemy != null)
            {
                Debug.Log("SHOOT");
                GameObject arrowInstance = Instantiate(arrow, transform.parent.transform.position, Quaternion.identity);
                arrowInstance.GetComponent<Rigidbody2D>().AddForce(firstEnemy.transform.position - arrowInstance.transform.position, ForceMode2D.Impulse);
            }
        }
        timer++;
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER");
        Debug.Log(collision.tag);
        if (collision.tag == "Enemy" && enableShoot)
        {
            if (enemies == null)
            {
                enemies.Add(collision.gameObject);
                timer = shootTime;
            } else
            {
                enemies.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && enableShoot)
        {
            enemies.Remove(collision.gameObject);
        }
    }
}
