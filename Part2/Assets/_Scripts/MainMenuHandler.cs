using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

    public InputField turns;
    public InputField board;

    public GameObject[] panels;
    int selectedIndex = 0;

    public void StartGame()
    {
        if(selectedIndex == 0)
        {
            Debug.Log("No board selected");
            return;
        }

        TurnManager.boardIndex = selectedIndex;


        if (Convert.ToInt32(turns.text) < 1)
        {
                    
            Debug.Log("Select a valid turn limit");
            return;
        }

        TurnManager.turnAmount = Convert.ToInt32(turns.text);
        TurnManager.currentTurn = 1;
        SceneManager.LoadScene(1);
    }

    public void SelectBoard(int index)
    {
        selectedIndex = index +1;
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#D27C7CFF", out color);
        foreach (GameObject go in panels)
        {
            go.GetComponent<Image>().color = color;

        }
        ColorUtility.TryParseHtmlString("#636363FF", out color);
        panels[index].GetComponent<Image>().color = color;

    }
}
