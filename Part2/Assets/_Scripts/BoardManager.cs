using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    //In editor assigments/ containers
    public GameObject[] BoardPrefabs;
    public GameObject[] PlayerPrefabs;
    public GameObject[] resourcePanels;

    public DialogueManager dialogueManager;
    public GameObject DiceCounter;
    public GameObject TurnBanner;
    public GameObject DiceBlock;

    
    //Containers
    private Player[] players;
    private GameObject loadedBoard;
    private GameObject spawnedDiceBlock;
    private GameObject Star;

    //States
    private bool turnEnd = true;
    private bool startTurnInput = false;

    //Settings
    public float starheight;
    private int numPlayers = 4;

    //Getter functions
    public List<Tile> getAllTileObjects()
    {
        List<Tile> AllTileObjects = new List<Tile>();
        foreach (Transform tile in this.transform.Find("Board" + TurnManager.boardIndex).Find("Tiles"))
        {
            AllTileObjects.Add(tile.GetComponent<Tile>());
        }
        return AllTileObjects;
    }
    public Tile getTileByIndex(int index)
    {
        Tile Tile = null;
        foreach (Transform tile in this.transform.Find("Board" + TurnManager.boardIndex).Find("Tiles"))
        {
            if (tile.GetComponent<Tile>().tileIndex == index)
            {
                Tile = tile.GetComponent<Tile>();
            }
        }
        return Tile;
    }
    public Player getCurrentPlayer()
    {
        if (TurnManager.currentPlayerIndex > -1 && TurnManager.currentPlayerIndex < 4)
        {
            return players[TurnManager.currentPlayerIndex];
        }
        return null;
    }
    public Tile getTileByType(TileType type)
    {
        //Searches by type, usually will be used for finding star/ begintile

        Tile Tile = null;
        foreach (Transform tile in this.transform.Find("Board" + TurnManager.boardIndex).Find("Tiles"))
        {
            if (tile.GetComponent<Tile>().tileType == type)
            {
                Tile = tile.GetComponent<Tile>();
            }
        }
        return Tile;
    }
    public Tile[] GeneratePath(Tile currentTile, int movement, bool branch = false)
    {
        Tile[] tilePath = new Tile[movement];

        //Generates a path from currentile to currenttile + movement.

        int n = 0;
        int nextTileIndex = currentTile.getNextTileIndex();
        int currentTileIndex = currentTile.tileIndex;
        Tile currentForTile = currentTile;

        for (int i = currentTileIndex; i < currentTileIndex + movement; i++)
        {
            #region branching
            //If tile has a branchindex specified and branch wasnt specified and the branch isnt a branchtile. This means this is the end of a branching pathway.
            //The specified branchindex is the reentering point for the player into the normal pathway
            //So the nextile will be the branch tile
            //This is also used to wrap around the map - branchindex of the last tile is specified as 1, so the player can restart the loop
            if (currentForTile.branchIndex > -1 && branch == false && currentForTile.tileType != TileType.BranchTile)
            {
                nextTileIndex = currentForTile.branchIndex;
            }
            //If branch was specified that means we are on a branchtile, and the player wants to go onto the branch.
            //So the nextile will be the branch tile
            else if (currentForTile.branchIndex > -1 && branch == true)
            {
                nextTileIndex = currentForTile.branchIndex;
            }
            else
            {
                //The player wants/has to stay on the normal pathway.
                //The nexttile will just be the normal next index
            }
            #endregion

            tilePath[n] = getTileByIndex(nextTileIndex);


            //If no tile was found log an error
            if (tilePath[n] == null)
            {
                Debug.LogError(String.Format("NULLTILE at index {2}: Presumed index = {0}, Or presumed BranchIndex = {1}", nextTileIndex, currentTile.branchIndex, n));
            }


            currentForTile = getTileByIndex(nextTileIndex);
            nextTileIndex++;
            n++;
        }



        //--tilePath debugger, to see whats the final state and items of tilePath array
        //foreach (Tile tile in tilePath)
        //{
        //    if (tile == null)
        //    {
        //        Debug.Log("Tile == null");
        //    }
        //    else
        //    {
        //        Debug.Log("Tileindex in patharray: " + tile.tileIndex);

        //    }
        //}

        //Return the genrated tilepath
        return tilePath;
    }

    //Initializing functions
    void Start()
    {
        LoadBoard();
        if (loadedBoard == null)
        {
            Debug.LogError("ERROR - No Board could be loaded!!!");
            return;
        }

        players = new Player[numPlayers];



        SpawnPlayers();


        //The turn loop
        //The turn loop, is the loop this game will follow to handle turns
        //It will start here at turnLoop part 0
        //After that though it will loop from turnLoop 1, to turnLoop 4, back to TurnLoop 1 etc.
        //This will help better visualize the code, and expand the game
        //A turn =/= turnLoop. A turnLoop is 4 player turns!!!
        //Turns will also loop within the TurnLoop. This happens from TurnLoop part 1 to TurnLoop part 3

        //TurnLoop part 0: Starts the turn loop

        StartCoroutine(StartTurn());

    }
    private void LoadBoard()
    {
        loadedBoard = Instantiate(BoardPrefabs[TurnManager.boardIndex -1], this.transform);
        loadedBoard.name = "Board" + TurnManager.boardIndex;
        Star = loadedBoard.transform.Find("Star").gameObject;
        if(Star == null)
        {
            Debug.Log("No star found");
        }
        else
        {
            MoveStarToRandomLegalLocation();
        }
        TurnManager.GameProperlyLoaded = true;

    }
    public void SpawnPlayers()
    {

        Tile begintile = getTileByType(TileType.BeginTile);
        if (begintile == null)
        {
            Debug.LogError("No tile of TileType: BeginTile found, can't spawn players!");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            players[i] = Instantiate(PlayerPrefabs[i], begintile.transform.Find("PlayerPlaceMarkers").Find("Player" + (i + 1) + "PlaceMarker").position, begintile.transform.rotation).GetComponent<Player>();
            players[i].setupPlayer(i, begintile.GetComponent<Tile>(), dialogueManager, this, resourcePanels[i]);
            players[i].playerResources.UpdateUI();
        }
    }

    public void SpawnDiceBlock()
    {
        spawnedDiceBlock = Instantiate(DiceBlock, new Vector3(getCurrentPlayer().transform.position.x, getCurrentPlayer().transform.position.y + 1.5f, getCurrentPlayer().transform.position.z), Quaternion.identity, getCurrentPlayer().transform);
    }

    //Turnloop
    IEnumerator StartTurn()
    {
        //TurnLoop part 1: Starts a turn
        //This code will play before a player has inputted anything
        //Right now show the turnbanner
        TurnBanner.SetActive(true);
        TurnBanner.transform.Find("BannerText").GetComponent<Text>().text = "player " + (TurnManager.currentPlayerIndex + 1) + " turn!";

        while(startTurnInput == false)
        {
            yield return null;
        }
        SpawnDiceBlock();
        while (TurnManager.TurnInProgress == false)
        {
            yield return null;
        }


        ////TEST
        //StartPlayerTurn();
        yield return new WaitForSeconds(1f);
        ////TEST
        //StartPlayerTurn();
    }



    public void StartPlayerTurn()
    {
        if(startTurnInput == false && TurnManager.TurnInProgress == false && TurnManager.MinigameInProgress == false)
        {
            startTurnInput = true;
            TurnBanner.SetActive(false);


            return;
        }

        if (startTurnInput == true && TurnManager.TurnInProgress == false & turnEnd == true && TurnManager.MinigameInProgress == false)
        {
            //TurnLoop part 2: Roll die and move player
            //The player has showed us that he wants to roll his die.
            //This part of the turnLoop will roll the die and handle player movement and events after that.
            //A coroutine (BoardManager.EndTurn) will be waiting for player to say he is done with computing the turn, or that is for (TurnManager.TurnInProgress) to be false. 
            //Which (BoardManager.EndTurn) will be the next part of the turn loop


            //turnEnd is a state just to fix a bug between the gap of turninprogress and playerswitching
            //turninprogress doesnt get set to false at the absolute end of a turn but before switching players (and has to keep doing that)
            //Thats why another state was necessary to have an absolute turnend state
            Destroy(spawnedDiceBlock);
            turnEnd = false;
            startTurnInput = false;
            TurnManager.TurnInProgress = true;


            //Set camera mode to follow the players
            CameraBehaviour.cameraMode = CameraMode.FollowPlayer;


            //Create path for players to move through
            int movement = TurnManager.RollDice();




            // setactive the DiceCounter, and give it to the player via movethroughtiles, then updates every step and disable again
            //TODO: give it start animation, but that requires optimizing turns first
            DiceCounter.transform.Find("CounterVisual").GetComponent<TextMeshPro>().text = movement.ToString();
            DiceCounter.SetActive(true);

            Tile currentTile = players[TurnManager.currentPlayerIndex].CurrentTile;


            //Tell players to move
            players[TurnManager.currentPlayerIndex].MovePlayerThrougTiles(GeneratePath(currentTile, movement), movement, DiceCounter);



            StartCoroutine(EndTurn());
        }
    }
    IEnumerator EndTurn()
    {
        //TurnLoop part 3
        //This is the last thing that happens every turn
        //While will wait for player to say he is done computing the turn
        //Then this will check if there is another player in line for a turn
        //if that is the case, the turnLoop will loop back to part 1. (BoardManager.StartTurn)
        //If all players had there turn, this will lead to the final part of the turnloop (BoardManager.StartMiniGame)
        //which will in turn also lead back to part 1. (BoardManager.StartTurn)
        while (TurnManager.TurnInProgress == true)
        {
            yield return null;
        }
        
        //reset current player index, when every player had it's turn, else starts a new turn for the next player
        //BUG! Sometimes a player can move twice by hammering the roll turn button
        //Fixed with turnEnd state
        if (TurnManager.currentPlayerIndex == 3)
        {
            TurnManager.currentPlayerIndex = 0;
            CameraBehaviour.cameraMode = CameraMode.MapViewMode;
            TurnManager.MinigameInProgress = true;

            ////TEST, loop game without user input
            //StartMiniGame();
        }
        else
        {
            TurnManager.currentPlayerIndex++;
            StartCoroutine(StartTurn());
        }


        CalcAndUpdatePlayerRankings();
        turnEnd = true;
        startTurnInput = false;

        ////TEST, loop game without user input
        //StartPlayerTurn();
        

    }
    public void StartMiniGame()
    {

        //TurnLoop part 4
        //This is the last thing that happens within a turnloop
        //A minigame will be played
        //Once it is finished it returns to TurnLoop part 1, to BoardManager.StartTurn()
        //And player 1's turn will be started again

        if (TurnManager.MinigameInProgress == true)
        {
            //Play minigame here
            Debug.Log("Minigame playing");


            //End of minigame

            //Award winner with coins
            //For now we simulate this randomly
            players[UnityEngine.Random.Range(0, players.Length)].playerResources.AddCoinsToCoinCount(10);

            TurnManager.MinigameInProgress = false;
            Debug.Log("Minigame ended");
            CameraBehaviour.cameraMode = CameraMode.FollowPlayer;

            //Start turn for player 1
            StartCoroutine(StartTurn());

        }
    }

    //Update Functions
    public void CalcAndUpdatePlayerRankings()
    {
        //TODO:
        //Recalculates the player rankings and updates it in their respective resourcePanels

        Dictionary<Player, int> playerStars = new Dictionary<Player, int>();

        foreach (Player player in players)
        {
            playerStars.Add(player, player.getStars());
        }

        var myList = playerStars.ToList();

        myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        int i = 1;
        foreach (KeyValuePair<Player, int> entry in playerStars)
        {
            entry.Key.setRank(i);
            i++;
        }


        //int rankToInc = 0;


        //foreach (Player player in players)
        //{
        //    foreach (Player playerCompare in players)
        //    {
        //        if (player.getStars() <= playerCompare.getStars())
        //        {
        //            rankToInc++;
        //        }
        //    }
        //    player.setRank(rankToInc);
        //    rankToInc = 0;
        //}
    }
    public void MoveStarToRandomLocation()
    {
        List<Tile> tiles = getAllTileObjects();

        //Find current startile and change it into a normal blue tile
        //FIXME: This will essentially purify red tiles, which is probably not what we want
        getTileByType(TileType.StarTile).ChangeIntoTile(TileType.BlueTile);

        //Get a new random tile to make a star tile of
        Tile newStarTile = null;
        do
        {
            newStarTile = tiles.ElementAt(UnityEngine.Random.Range(0, tiles.Count));
        }
        while (newStarTile.tileType == TileType.BeginTile || newStarTile.tileType == TileType.BranchTile);
        
                
        
       // Tile newStarTile = tiles.ElementAt(UnityEngine.Random.Range(0, tiles.Count));
        
        newStarTile.ChangeIntoTile(TileType.StarTile);

        //TODO: animate star to this new tile
        Star.transform.position = newStarTile.transform.position + (Vector3.up * 1.5f);
    }
    public void MoveStarToRandomLegalLocation(int currentStarTileIndex  = -1)
    {
        //Gets a legal random startile from boarddata
        int newTileIndex = loadedBoard.GetComponent<BoardData>().getRandomStarTile(currentStarTileIndex);
        Tile newStarTile = getTileByIndex(newTileIndex);

        if (newStarTile.tileType == TileType.BeginTile || newStarTile.tileType == TileType.BranchTile)
        {
            Debug.LogError(String.Format("Cant move to star to specified tile with index: {0}, because tile is a {1}", newTileIndex, newStarTile.tileType.ToString()));
            return;
        }

        //Find current startile and change it into a normal blue tile
        getTileByType(TileType.StarTile).ChangeIntoTile(TileType.BlueTile);

        //Change new startile into starTile
        newStarTile.ChangeIntoTile(TileType.StarTile);

        //Sets the actual star location
        //TODO: animate star to this new tile
        Star.transform.position = newStarTile.transform.position + (Vector3.up * starheight);

        //Debug star vs new tile position
        Debug.Log("Star: X- " + Star.transform.position.x + "  y- " + Star.transform.position.y + "  z- " + Star.transform.position.z);
        Debug.Log("New Tile: X- " + newStarTile.transform.position.x + "  y- " + newStarTile.transform.position.y + "  z- " + newStarTile.transform.position.z);

    }
}