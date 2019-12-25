using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected bool isPlaced = false;
    protected bool canPlace = false;
    protected SpriteRenderer sprite;

    private Money moneyScript;

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        moneyScript = GameObject.FindGameObjectWithTag("Money").GetComponent<Money>();
    }

    protected virtual void Update()
    {
        if (!isPlaced)
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0;
            transform.parent.transform.position = vec;
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                isPlaced = true;
                TowerManager.currentBuilding = null;
                moneyScript.subtractMoney(10);
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.tag == "Waypoint" || collision.transform.tag == "Tower") && !isPlaced)  // if overlapping onto path or other building
        {
            sprite.color = Color.red;
            canPlace = false;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.transform.tag == "Waypoint" || collision.transform.tag == "Tower") && sprite.color != Color.red && !isPlaced)
        {
            sprite.color = Color.red;
            canPlace = false;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Waypoint" || collision.transform.tag == "Tower")
        {
            sprite.color = Color.white;
            canPlace = true;
        }
    }
}
