using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_BackToStart : IEvent {

    Player player;


    private Transform startMarker;
    private Transform endMarker;
    private float speed = 6F;
    private float startTime;
    private float journeyLength;
    float fracJourney;

    private bool startLerping;


    public override void runEvent(Player player)
    {
       

        this.player = player;
        startMarker = player.transform;
        Tile beginTile = GetComponentInParent<BoardManager>().getTileByType(TileType.BeginTile);
        endMarker = beginTile.transform.Find("PlayerPlaceMarkers").Find("Player" + (player.playerIndex + 1) + "PlaceMarker");
        player.CurrentTile = beginTile;

        startLerp();
        //Tile[] tiles = new Tile[1];
        //tiles[0] = GetComponentInParent<BoardManager>().getTileByType(TileType.BeginTile);
        //player.MovePlayerThrougTiles(tiles, 1);
        //player.transform.SetParent(GetComponentInParent<BoardManager>().transform);

    }

    void startLerp()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        startLerping = true;
    }

    void Update()
    {
        if (startLerping)
        {
            float distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;
            player.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        }

        //??? - 0,2?
        if(fracJourney > 0.25f && startLerping)
        {
            player.transform.position = endMarker.position;

            TurnManager.TurnInProgress = false;
            Debug.Log("Event finished");
            startLerping = false;

        }

    }
}
