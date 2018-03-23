using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject MenuPanel;
    public GameObject MainMenuPanel;
    public GameObject ButtonRollDice;
    public GameObject turnCounter;

    void Awake()
    {
        updateTurnCounter();
    }


    public void btnBack_onclick()
    {
        MenuPanel.SetActive(false);
        UnPauseGame();
    }
    public void btnOptions_onclick()
    {

    }
    public void btnQuit_onclick()
    {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
    public void btnMainMenu_onclick()
    {
        QuitToMainMenu();
    }

    public void PauseGame()
    {
        ButtonRollDice.SetActive(false);
        TurnManager.gamePaused = true;
        MainMenuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        ButtonRollDice.SetActive(true);
        TurnManager.gamePaused = false;
        MainMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void updateTurnCounter()
    {
        turnCounter.GetComponent<Text>().text = TurnManager.currentTurn + " / " + TurnManager.turnAmount;
    }


    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToResultsScreen()
    {
        SceneManager.LoadScene(2);
    }
}
