using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // In editor assigments/ containers
    public int playerIndex;
    public DiceBlock DiceBlock;
    public GameObject MoveCounter;
    public Transform CameraPosition;
    [System.NonSerialized] public PlayerResources playerResources;

    public bool AIControlled = false;

    //Containers
    DialogueManager dialogueManager;
    BoardManager boardManager;

    [System.NonSerialized] public Tile CurrentTile;
    Tile targetTile;
    Tile[] Path;

    [System.NonSerialized] public int movement;
    [System.NonSerialized] public int moved = 0;

    //States
    [System.NonSerialized] public bool lastInputWasYes = false;

    //Settings
    #region Lerp Values

    //!!!: these lerp values may not be optimized for user experience but rather for testing
    //TODO: get these into unity editor

    //Good debug values: PLT: 0.2f - WBM: 0.01f - RLT: 0.1f
    //good? user values: PLT: 0.3f - WBM: 0.2f - RLT: 0.2f
    float positionLerpTime = 0.3f;
    float waitBetweenMoves = 0.15f;
    float rotationLerpTime = 0.2f;
    float currentLerpTime = 0;

    float cameraFaceLerpTime = 0.2f;
    float currentFaceCameraLerp = 0f;

    float waitBetweenTurns = 1f;

    Quaternion startSlerpForMoving;

    Quaternion startRotationForCameraFace;
    Quaternion endRotation;


    Vector3 startPosition;
    Vector3 targetPosition;
    private bool endTurn;

    #endregion

    //Getter/Setter functions
    public int getCoins()
    {
        return playerResources.CoinCount;
    }
    public int getStars()
    {
        return playerResources.StarCount;
    }
    public int getRank()
    {
        return playerResources.Rank;
    }
    public void setRank(int rank)
    {
        playerResources.Rank = rank;
    }
    public void SetActiveDiceBlock(bool active)
    {
        DiceBlock.gameObject.SetActive(active);
    }

    public void SpendCrystal()
    {
        if(getCoins() >= 5)
        {
            playerResources.SpendCrystal();
            Debug.Log("Player.spendCrystal");
        }
    }

    //Initializing functions
    public void setupPlayer(int index, Tile StartTile, DialogueManager DM, BoardManager BM, GameObject resourcePanel)
    {
        // Gives player his unique data, and some references

        CurrentTile = StartTile;
        playerIndex = index;
        dialogueManager = DM;
        boardManager = BM;
        playerResources = GetComponent<PlayerResources>();


        playerResources.ResourcesPanel = resourcePanel;

        if (TurnManager.SpeedyTestMode)
        {
            dialogueManager.timedDialogue = 0f;

            positionLerpTime = 0f;
            waitBetweenMoves = 0f;
            rotationLerpTime = 0f;
            cameraFaceLerpTime = 0f;
            waitBetweenTurns = 0f;
        }
    }

    //TurnLoop
    public void MovePlayerThrougTiles(Tile[] path, int movement)
    {
        MoveCounter.SetActive(true);
        MoveCounter.transform.Find("CounterVisual").GetComponent<TextMeshPro>().text = movement.ToString();

        Path = path;

        //movement will decide if we will move one more tile
        this.movement = movement;
        //Debug.Log("Movement: " + movement);
        //moved will advance our path array
        this.moved = 0;

        //Start animating player to the target tile
        StartAnimating();
    }
    void StartAnimating()
    {
        //Set the target to be the next in the list, based on how much we already moved
        targetTile = Path[moved];

        //if there is no tile to move to, bail.  This happens only as an error. 
        //The end of the CR will check if player has movement left and bails there.
        if (targetTile == null)
        {
            TurnManager.TurnInProgress = false;
            Debug.LogError("targettile == null, bailed animation---Moved: " + moved);
            Debug.Log("Tile indexes in path:");
            if (Path == null)
            {
                Debug.Log("Path == null");
                return;
            }
            foreach (Tile tile in Path)
            {
                if (tile == null)
                {
                    Debug.Log("Tile == null");
                }
                else
                {
                    Debug.Log("Foreach pathTile: " + tile.tileIndex);
                }

            }
            return;
        }

        //Log some usefull info
        //Debug.Log("Animate from CurrentTile: " + CurrentTile.tileIndex + " To  ->  TargetTile: " + targetTile.tileIndex);


        //We can't just move to the tile's transform, we need to move to the spot reserved for our player
        startPosition = CurrentTile.transform.Find("PlayerPlaceMarkers").Find("Player" + (playerIndex + 1) + "PlaceMarker").position;
        targetPosition = targetTile.transform.Find("PlayerPlaceMarkers").Find("Player" + (playerIndex + 1) + "PlaceMarker").position;

        //reset the current lerp, so it's always fresh before it starts the coroutine
        currentLerpTime = 0;


        //Start the CR
        StartCoroutine(CRAnimatePlayer());
    }

    IEnumerator CRAnimatePlayer()
    { 

        #region AnimationPhase

        //Move until we reach our target position
        while (currentLerpTime < positionLerpTime)
        {
            float posPercentage = currentLerpTime / positionLerpTime;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, posPercentage);

            float rotPercentage = currentLerpTime / rotationLerpTime;

            //???: is this necessary?
            if (moved == 0)
            {
                startSlerpForMoving = this.transform.rotation;
            }
            else
            {
                startSlerpForMoving = CurrentTile.transform.rotation;
            }

            this.transform.rotation = Quaternion.Slerp(startSlerpForMoving, targetTile.transform.rotation, rotPercentage);

            
            currentLerpTime += Time.deltaTime;
            yield return null;
        }
        //The lerp doesnt actually take the player all the way to targetPosition. 
        //So to avoid this error, I set it here at the end, and it still looks smooth.
        this.transform.position = targetPosition;

        ////DiceCounter.transform.position = this.transform.position + (Vector3.up * 1.5f);



        //We finished the move, now we have less movement but we moved more. Also our currenttile is now the targettile we moved to.

        //Branch tile and star tiles ignores 1 movement, so if targetile is a branchtile dont lower movement
        if(targetTile.tileType != TileType.BranchTile && targetTile.tileType != TileType.StarTile && targetTile.tileType != TileType.ShopTile)
        {
            movement--;
        }
        moved++;
        CurrentTile = targetTile;

        //update moveCounter
        MoveCounter.transform.Find("CounterVisual").GetComponent<TextMeshPro>().text = movement.ToString();

        #endregion


        #region CheckAndHandleTileEvents
        //Check if this is a star tile, In case it is, trigger star dialogue
        //Prompt the player a choice to buy the star for 20 coins
        //if they have less than twenty coins show a special dialogue
        //that will automatically bail out.
        if (CurrentTile.tileType == TileType.StarTile)
        {
            //disable dicecounter, else its in the way of the star
            MoveCounter.SetActive(false);


            //triggers dialogue and waits for user input, after that player can move again
            TurnManager.DialogueInProgress = true;
            dialogueManager.TriggerDialogue(this, CurrentTile, AIControlled);
            yield return StartCoroutine(WaitForUserInput());

            //if player answered yes, star is bought and star is relocated.
            if (lastInputWasYes)
            {
                playerResources.BuyStar();
                boardManager.MoveStarToRandomLegalLocation(CurrentTile.tileIndex);
            }

            //We need to regenerate path, because star tile ignores movement. this means the path just got 1 tile longer
            moved = 0;
            Path = boardManager.GeneratePath(CurrentTile, movement);

            //enable moveCounter again
            MoveCounter.SetActive(true);
        }
        //Check if there is a shop tile
        //Player is promtped to buy a crystal for 5 coins
        else if (CurrentTile.tileType == TileType.ShopTile)
        {
            //disable dicecounter, else its in the way of the shop
            MoveCounter.SetActive(false);


            //triggers dialogue and waits for user input, after that player can move again
            TurnManager.DialogueInProgress = true;
            dialogueManager.TriggerDialogue(this, CurrentTile, AIControlled);
            yield return StartCoroutine(WaitForUserInput());

            //if player answered yes, shop is bought and star is relocated.
            if (lastInputWasYes)
            {
                playerResources.BuyCrystal();
            }

            //We need to regenerate path, because shop tile ignores movement. this means the path just got 1 tile longer
            moved = 0;
            Path = boardManager.GeneratePath(CurrentTile, movement);

            //enable moveCounter again
            MoveCounter.SetActive(true);
        }
        //We check here if a tile is a branch tile
        //And we offer the player a choice and change (or dont change) the path of the player
        else if (CurrentTile.tileType == TileType.BranchTile)
        {
            moved = 0;

            //Triggers dialogue and wait for user input, after that player will move again
            TurnManager.DialogueInProgress = true;
            dialogueManager.TriggerDialogue(this, CurrentTile, AIControlled);
            yield return StartCoroutine(WaitForUserInput());

            //Yes represents branch, and no represents dont branch
            if (lastInputWasYes)
            {
                //Move down the branch by generating new path
                Path = boardManager.GeneratePath(CurrentTile, movement, true);
            }
            else
            {
                //Dont move down the branch by generating new path
                Path = boardManager.GeneratePath(CurrentTile, movement);
            }
        }


        #endregion


        #region HandleTurnEnd


        //Check if we have movement left
        if (movement > 0)
        {
            //Wait a bit, and Animate again
            yield return new WaitForSeconds(waitBetweenMoves);
            StartAnimating();

        }
        else
        {
            //turn player around
            currentFaceCameraLerp = 0f;
            startRotationForCameraFace = this.transform.rotation;
            endRotation = Quaternion.AngleAxis(-90, transform.up);
            StartCoroutine( CRLerpPlayerToFaceCamera());




            MoveCounter.SetActive(false);
            yield return new WaitForSeconds(waitBetweenTurns);

            //We finished moving
            StartTurnEnd();


        }
        #endregion
    }
    IEnumerator CRLerpPlayerToFaceCamera()
    {

        while (currentFaceCameraLerp < cameraFaceLerpTime)
        {
            float rotPercentage = currentFaceCameraLerp / cameraFaceLerpTime;
            this.transform.rotation = Quaternion.Slerp(startRotationForCameraFace, endRotation, rotPercentage);
            
            currentFaceCameraLerp += Time.deltaTime;
            yield return null;
        }
        this.transform.rotation = endRotation;
    }

    IEnumerator WaitForUserInput()
    {
        while (TurnManager.DialogueInProgress)
        {
            yield return null;
        }
    }

    void StartTurnEnd()
    {
        //Check if final tile is blue/begin, red or star
        //if blue give player 3 coins, if red subtract 3 coins

        switch (CurrentTile.tileType)
        {
            case TileType.BeginTile:
                //Nothing happens on begintile
                TurnManager.TurnInProgress = false;
                break;
            case TileType.BlueTile:
                playerResources.CoinCount += 3;
                TurnManager.TurnInProgress = false;
                break;
            case TileType.RedTile:
                playerResources.CoinCount -= 3;
                TurnManager.TurnInProgress = false;
                break;
            case TileType.StarTile:
                //nothing happens when you end a turn on a star, since the star prompt already happened
                TurnManager.TurnInProgress = false;
                break;
            case TileType.ShopTile:
                //nothing happens when you end a turn on a shop, since the shop prompt already happened
                TurnManager.TurnInProgress = false;
                break;
            case TileType.EventTile:
                //Play an event
                CurrentTile.TileEvt.runEvent(this);
                break;
            default:
                TurnManager.TurnInProgress = false;
                break;
        }
    }

    //Update functions
    void LateUpdate()
    {
        MoveCounter.transform.rotation = Quaternion.identity;
    }
}
