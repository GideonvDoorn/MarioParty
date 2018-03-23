using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_BackToStart : IEvent {

    Player player;



    public override void runEvent(Player player)
    {
       

        this.player = player;


        Tile[] tiles = new Tile[1];
        tiles[0] = GetComponentInParent<BoardManager>().getTileByType(TileType.BeginTile);
        player.MovePlayerThrougTiles(tiles, 1);
        player.transform.SetParent(GetComponentInParent<BoardManager>().transform);

    }

    void Update()
    {

    }
}
