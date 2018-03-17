using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour {

    public int Amount;
    public int StartAmount;
    public Text resourceText;

    public void AddAmount(int amount)
    {
        if(amount < 0)
        {
            Debug.LogError("Amount must be positive!");
            return;
        }
        this.Amount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        resourceText.text = Amount.ToString();
    }
}
