using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    static bool isBuilding = false;
    public static GameObject currentBuilding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called by TowerButton to decide what building we are trying to build
    public static void SetTower()
    {
        isBuilding = true;
    }
}
