using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int currentHealth = 100;
    private Text textComponent;

    public void loseHealth(int amount)
    {
        currentHealth -= amount;
        textComponent.text = currentHealth.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
