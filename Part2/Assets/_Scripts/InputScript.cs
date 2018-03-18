using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    BoardManager BM;

    void Awake()
    {
        BM = GetComponent<BoardManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!TurnManager.gamePaused)
            {
                BM.StartPlayerTurn();

            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TurnManager.gamePaused)
            {
                BM.UnPauseGame();
            }
            else
            {
                BM.PauseGame();
            }

        }

    }
}