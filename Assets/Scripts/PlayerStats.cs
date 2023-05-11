using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats 
{

    public static int Money { get; private set; }

    public const string CURRENCY = "G"; 

    public static void Spend(int cost)
    {
        if(cost > Money)
        {
            Debug.LogError("Player dose not have enough money");
        }
        Money -= cost;
        UIManager.Instance.RenderPlayerStats();
    }

    public static void Earn(int income)
    {
        Money += income;
        UIManager.Instance.RenderPlayerStats();

    }

}
