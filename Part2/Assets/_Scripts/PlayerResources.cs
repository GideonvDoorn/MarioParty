using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour {

    //Keeps track of player resources and translates this to UI

    private int rank = 4;
    private int starCount = 0;
    private int coinCount = 20;
    private int crystals = 2;

    [System.NonSerialized] public GameObject ResourcesPanel; 

    public int StarCount
    {
        get
        {
            return starCount;
        }
        set
        {
            starCount = value;
            UpdateUI();
        }
    }

    public int CoinCount
    {
        get
        {
            return coinCount;
        }
        set
        {
            if(value < 0)
            {
                CoinCount = 0;
            }
            else
            {
                coinCount = value;
            }
            UpdateUI();
        }
    }

    public int Rank
    {
        get
        {
            return rank;
        }

        set
        {
            rank = value;
            UpdateUI();
        }
    }

    public int Crystals
    {
        get
        {
            return crystals;
        }

        set
        {
            crystals = value;
            UpdateUI();
        }
    }

    public void AddCoinsToCoinCount(int coinsToAdd)
    {
        CoinCount += coinsToAdd;
    }

	public void UpdateUI () {
        Text t = ResourcesPanel.transform.Find("CountersPanel").Find("StarCount").GetComponent<Text>();
        Text t2 = ResourcesPanel.transform.Find("CountersPanel").Find("CoinCount").GetComponent<Text>();
        Text t3 = ResourcesPanel.transform.Find("CountersPanel").Find("CrystalCount").GetComponent<Text>();
        Text t4 = ResourcesPanel.transform.Find("RankText").GetComponent<Text>();
        t.text = "Stars : " + StarCount;
        t2.text = "Coins : " + CoinCount;
        t3.text = "Crystals : " + Crystals;
        t4.text = Rank.ToString();
    }

    public void BuyStar()
    {
        StarCount++;
        CoinCount -= 20;
    }

    public void BuyCrystal()
    {
        Crystals++;
        CoinCount -= 5;
    }

    public void SpendCrystal()
    {
        if(Crystals == 0)
        {
            return;
        }
        Crystals--;
        TurnManager.DiceRollHeight += 5;
    }
}
