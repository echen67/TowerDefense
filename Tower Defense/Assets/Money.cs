using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    private int currentMoney = 0;
    private Text textComponent;

    public void addMoney(int amount)
    {
        currentMoney += amount;
        textComponent.text = currentMoney.ToString();
    }

    public void subtractMoney(int amount)
    {
        currentMoney -= amount;
        textComponent.text = currentMoney.ToString();
    }

    void Start()
    {
        textComponent = GetComponent<Text>();
    }
}
