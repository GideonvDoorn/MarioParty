using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    BoardManager BM;
    UIManager UIM;

    void Awake()
    {
        BM = GetComponent<BoardManager>();
        UIM = BM.UIM;
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
                UIM.UnPauseGame();
            }
            else
            {
                UIM.PauseGame();
            }

        }

    }
}