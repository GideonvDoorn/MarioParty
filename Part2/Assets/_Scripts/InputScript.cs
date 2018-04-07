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
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!TurnManager.gamePaused)
            {
                BM.ExploreMapMode();

            }

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!TurnManager.gamePaused)
            {
                BM.BuyCrystal();

            }

        }

        if (CameraBehaviour.cameraMode == CameraMode.ExploreMap)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                Camera.main.GetComponent<CameraBehaviour>().scrollCamera(false);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                Camera.main.GetComponent<CameraBehaviour>().scrollCamera(true);
            }
        }


    }
}