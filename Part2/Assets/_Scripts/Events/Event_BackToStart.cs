using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_BackToStart : IEvent {


    public override void runEvent(Player player)
    {
        Debug.Log("Back to start");

        Tile[] tiles = new Tile[1];
        tiles[0] = GetComponentInParent<BoardManager>().getTileByType(TileType.BeginTile);

        player.MovePlayerThrougTiles(tiles, 1);
    }
}
