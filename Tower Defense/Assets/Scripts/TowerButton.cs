using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public GameObject tower;

    public void PlaceTower()
    {
        if (TowerManager.currentBuilding != null)
        {
            // destroy current building?
            Destroy(TowerManager.currentBuilding);
        }
        TowerManager.currentBuilding = Instantiate(tower);
    }
}
