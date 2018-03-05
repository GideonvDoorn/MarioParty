using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour{

    public int tileIndex;
    public int branchIndex = -1;
    public TileType tileType;

    public int getNextTileIndex()
    {
        return tileIndex + 1;
    }

    public void ChangeIntoTile(TileType tile)
    {

        //TODO: Should change material instead of color
        Renderer tileMat = transform.Find("TileVisual").GetComponent<Renderer>();
        switch (tile)
        {
            case TileType.BeginTile:
                //should never happen, so this method will do nothing
                break;
            case TileType.BlueTile:
                this.tileType = tile;
                tileMat.material.SetColor("_Color", Color.blue);
                break;
            case TileType.RedTile:
                this.tileType = tile;
                tileMat.material.SetColor("_Color", Color.red);
                break;
            case TileType.StarTile:
                this.tileType = tile;
                tileMat.material.SetColor("_Color", Color.yellow);
                break;
            case TileType.BranchTile:
                this.tileType = tile;
                tileMat.material.SetColor("_Color", Color.magenta);
                break;
            default:
                break;
        }
    }
}
