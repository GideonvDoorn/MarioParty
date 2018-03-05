using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

    public InputField turns;
    public InputField board;

    public void StartGame()
    {
        TurnManager.boardIndex = Convert.ToInt32(board.text);
        TurnManager.turnAmount = Convert.ToInt32(turns.text);

        SceneManager.LoadScene(1);
    }
}
