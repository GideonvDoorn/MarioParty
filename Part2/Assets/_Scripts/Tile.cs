using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour{

    public int tileIndex;
    public int branchIndex = -1;
    public TileType tileType;

    public IEvent TileEvt;

    private BoardData boardData;

    public int getNextTileIndex()
    {
        return tileIndex + 1;
    }

    void Awake()
    {
        boardData = GetComponentInParent<BoardData>();
    }

    public void ChangeIntoTile(TileType tile)
    {
        //changes material and type of tile
        Renderer tileMat = transform.Find("TileVisual").GetComponent<Renderer>();
        switch (tile)
        {
            case TileType.BeginTile:
                //should never happen, so this method will do nothing
                break;
            case TileType.BlueTile:
                this.tileType = tile;
                //tileMat.material.SetColor("_Color", Color.blue);
                tileMat.material = boardData.BlueTileMaterial;
                break;
            case TileType.RedTile:
                this.tileType = tile;
                tileMat.material = boardData.RedTileMaterial;
                break;
            case TileType.StarTile:
                this.tileType = tile;
                tileMat.material = boardData.StarTileMaterial;
                break;
            case TileType.BranchTile:
                this.tileType = tile;
                tileMat.material = boardData.BranchTileMaterial;
                break;
            case TileType.EventTile:
                this.tileType = tile;
                tileMat.material = boardData.EventTileMaterial;
                break;
            default:
                break;
        }
    }
}
