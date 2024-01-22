using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    
    private int coins;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        coins = 20;
    }

    void Start()
    {
        GameManager.instance.UpdateCoinsDisplay(coins);
    }

    public bool CanAfford(int Price)
    {
        return coins >= Price;
    }

    public void SpendCoins(int amount)
    {
        if(amount <= coins)
        {
            coins -= amount;
            GameManager.instance.UpdateCoinsDisplay(coins);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        GameManager.instance.UpdateCoinsDisplay(coins);
    }

    public void ResetCoinsToInitialValue()
    {
        coins = 20;
        GameManager.instance.UpdateCoinsDisplay(coins);
    }
}
