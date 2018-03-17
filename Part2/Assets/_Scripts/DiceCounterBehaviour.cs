using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceCounterBehaviour : MonoBehaviour {

    public float timer;
    private float currentTime = 0;
    public int counterMaximum;
    private int currentCount = 0;
    

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timer)
        {
            //Up the counter
            incrementDiceCounter();

            currentTime = 0;
        }

    }

    private void incrementDiceCounter()
    {
        if(currentCount == counterMaximum)
        {
            currentCount = 1;
        }
        else
        {
            currentCount++;
        }

        GetComponent<TextMeshPro>().text = currentCount.ToString();

    }
}
