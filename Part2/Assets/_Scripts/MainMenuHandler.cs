using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

    public InputField turns;
    public InputField board;

    public GameObject[] BoardPanels;
    public GameObject[] TurnButtons;
    public GameObject[] PlayerButtons;
    public GameObject[] DifficultyButtons;

    int selectedBoardIndex = 0;
    int selectedTurnsIndex = 0;
    int selectedPlayersIndex = 0;
    int selectedDifficultyIndex = 0;

    void Start()
    {
        SelectBoard(0);
        SelectTurnAmount(0);
        SelectPlayerAmount(0);
        SelectDifficulty(0);
    }

    public void StartGame()
    {
        if(selectedBoardIndex == 0)
        {
            Debug.Log("No board selected");
            return;
        }

        TurnManager.boardIndex = selectedBoardIndex;

        switch (selectedTurnsIndex)
        {
            case 1:
                TurnManager.turnAmount = 20;
                break;
            case 2:
                TurnManager.turnAmount = 30;
                break;
            case 3:
                TurnManager.turnAmount = 40;
                break;
            default:
                break;
        }

        TurnManager.playerAmount = selectedPlayersIndex;

        TurnManager.hardDifficulty = false;
        if(selectedDifficultyIndex == 1)
        {
            TurnManager.hardDifficulty = true;
        }

      
        SceneManager.LoadScene(1);
    }

    public void SelectBoard(int index)
    {
        selectedBoardIndex = index +1;
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#636363FF", out color);
        foreach (GameObject go in BoardPanels)
        {
            go.GetComponent<Image>().color = color;

        }
        ColorUtility.TryParseHtmlString("#F68F8FFF", out color);
        BoardPanels[index].GetComponent<Image>().color = color;

    }

    public void SelectPlayerAmount(int index)
    {
        selectedPlayersIndex = index + 1;
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#636363FF", out color);
        foreach (GameObject go in PlayerButtons)
        {
            go.GetComponent<Image>().color = color;

        }
        ColorUtility.TryParseHtmlString("#F68F8FFF", out color);
        PlayerButtons[index].GetComponent<Image>().color = color;
    }

    public void SelectTurnAmount(int index)
    {
        selectedTurnsIndex = index + 1;
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#636363FF", out color);
        foreach (GameObject go in TurnButtons)
        {
            go.GetComponent<Image>().color = color;

        }
        ColorUtility.TryParseHtmlString("#F68F8FFF", out color);
        TurnButtons[index].GetComponent<Image>().color = color;
    }

    public void SelectDifficulty(int index)
    {
        selectedDifficultyIndex = index + 1;
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#636363FF", out color);
        foreach (GameObject go in DifficultyButtons)
        {
            go.GetComponent<Image>().color = color;

        }
        ColorUtility.TryParseHtmlString("#F68F8FFF", out color);
        DifficultyButtons[index].GetComponent<Image>().color = color;
    }
}
