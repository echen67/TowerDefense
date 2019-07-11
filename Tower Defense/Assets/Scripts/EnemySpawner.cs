using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1;   // drag first enemy type here

    public float timeBetween = 1f; // time between spawning each enemy in the same wave
    public int numEnemies = 5;
    int timer = 0;

    // assume only 1 wave for now

    void Start()
    {
        
    }

    void Update()
    {
        if (timer >= timeBetween && numEnemies > 0)
        {
            timer = 0;
            Instantiate(enemy1);
            numEnemies--;
        }
        timer++;
    }
}
