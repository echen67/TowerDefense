using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenchBread : Tower
{
    public GameObject arrow;    // set in inspector
    public int shootTime = 100;
    private int timer = 0;

    TowerRange towerRange;

    protected override void Start()
    {
        base.Start();
        shootTime = 100;
        towerRange = transform.parent.GetChild(1).GetComponent<TowerRange>();
        //Debug.Log(transform.parent.GetChild(1).GetComponent<TowerRange>());
    }

    protected override void Update()
    {
        base.Update();
        if (isPlaced)
        {
            towerRange.enableShoot = true;
        }
        //if (isPlaced && timer > shootTime)
        //{
        //    GameObject enemy = towerRange.FindFirstEnemy();
        //    if (enemy != null)
        //    {
        //        GameObject arrowInstance = Instantiate(arrow);
        //        arrowInstance.GetComponent<Rigidbody2D>().AddForce(enemy.transform.position - arrowInstance.transform.position, ForceMode2D.Impulse);
        //    }
        //    timer = 0;
        //}
        //timer++;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
