using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceBlock : MonoBehaviour {

    public GameObject AnimatedCounter;
    public GameObject SetCounter;

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
        if (currentCount == counterMaximum)
        {
            currentCount = 1;
        }
        else
        {
            currentCount++;
        }

        AnimatedCounter.transform.Find("CounterVisual").GetComponent<TextMeshPro>().text = currentCount.ToString();

    }

    public void SetActiveAnimatedDiceCounter(bool active)
    {
        AnimatedCounter.SetActive(active);
    }

    public void SetActiveSetDiceCounter(bool active)
    {
        SetCounter.SetActive(active);

    }

}
