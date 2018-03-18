using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static bool GameProperlyLoaded = false;

    public static int currentPlayerIndex = 0;
    public static int boardIndex = 3;
    public static int turnAmount = 20;
    public static int currentTurn = 0;
    public static bool TurnInProgress = false;
    public static bool DialogueInProgress;
    public static bool MinigameInProgress = false;

    public static bool AutomaticTestMode = false;
    public static bool SpeedyTestMode = false;

    public static int RollDice()
    {
        return Random.Range(1, 11);
    }
}
