using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static bool GameProperlyLoaded = false;

    public static int currentPlayerIndex = 0;
    public static int playerAmount = 4;
    public static bool hardDifficulty = false;
    public static int boardIndex = 2;
    public static int turnAmount = 100;
    public static int currentTurn = 1;
    public static bool TurnInProgress = false;
    public static bool DialogueInProgress;
    public static bool BranchDialogueInProgress;
    public static bool MinigameInProgress = false;
    public static bool gamePaused = false;

    public static bool AutomaticTestMode = false;
    public static bool SpeedyTestMode = false;

    public static int DiceRollHeight = 11;

    public static int RollDice()
    {
        return Random.Range(1, DiceRollHeight);
    }
}
