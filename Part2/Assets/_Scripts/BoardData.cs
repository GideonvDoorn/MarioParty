using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData : MonoBehaviour {

    public int[] LegalStarSpawnTiles;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getRandomStarTile(int lastStarTile)
    {
        int result = -1;


        //If last star tile is specified as -1, we assume that every star tile is legal
        //Else we find a star tile on a tile that is not the last star tile
        //This so the star never spwans in the same place twice
        if(lastStarTile == -1)
        {
            result = LegalStarSpawnTiles[Random.Range(0, LegalStarSpawnTiles.Length)];
        }
        else
        {
            do
            {
                result = LegalStarSpawnTiles[Random.Range(0, LegalStarSpawnTiles.Length)];
            }
            while (result == lastStarTile);
        }
        if(result == -1)
        {
            Debug.LogError("No valid random starTile found");
        }
        return result;
    }
}
