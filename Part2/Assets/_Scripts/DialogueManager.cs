using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour {

    public GameObject DialoguePanel;
    public GameObject DialogueButtonPanel;
    public Text DialogueText;

    public GameObject BranchUI;


    public Player playerinDialogue;

    public float timedDialogue= 2f;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //TODO: get rid of this
    public void TriggerDialogue(Player player, Tile tile, bool AI)
    {
        playerinDialogue = player;
        if (tile.tileType == TileType.StarTile)
        {
            StartCoroutine(TriggerStarDialogue(player.getCoins(), AI));
        }
        else if(tile.tileType == TileType.BranchTile)
        {
            TriggerBranchDialogue(tile, AI);
        }
        else if(tile.tileType == TileType.ShopTile)
        {
            StartCoroutine(TriggerShopDialogue(player.getCoins(), AI));
        }

    }

    private void TriggerBranchDialogue(Tile tile, bool AI)
    {
        TurnManager.BranchDialogueInProgress = true;
        BranchUI.transform.Find("BranchButton").Find("Text").GetComponent<Text>().text = tile.BranchButtonText ;
        BranchUI.transform.Find("DontBranchButton").Find("Text").GetComponent<Text>().text = tile.DontBranchButtonText;
        BranchUI.SetActive(true);

        if (TurnManager.AutomaticTestMode || AI)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                TriggerYes();
            }
            else
            {
                TriggerNo();
            }
        }
    }

    public IEnumerator TriggerShopDialogue(int playerCoins, bool AI)
    {
        DialoguePanel.SetActive(true);

        if (playerCoins <5)
        {
            DialogueButtonPanel.SetActive(false);
            DialogueText.text = String.Format("Want to buy a crystal? You currently have {0} coins, you can't buy it!", playerCoins);
            yield return new WaitForSeconds(timedDialogue);
            DialoguePanel.SetActive(false);

            TurnManager.DialogueInProgress = false;
            playerinDialogue.lastInputWasYes = false;
        }
        else
        {
            DialogueButtonPanel.SetActive(true);
            DialogueText.text = String.Format("Want to buy a crystal? It costs 5 coins! You currently have {0} coins!", playerCoins);

            ////TEST, loop turn without player input
            //TriggerYes();
            if (TurnManager.AutomaticTestMode || AI)
            {
                TriggerYes();
            }
        }


    }


    public IEnumerator TriggerStarDialogue(int playerCoins, bool AI)
    {
        DialoguePanel.SetActive(true);


        if (playerCoins < 20)
        {
            DialogueButtonPanel.SetActive(false);
            DialogueText.text = String.Format("You found a star! It costs 20 coins! You currently have {0} coins, you can't buy it!", playerCoins);
            yield return new WaitForSeconds(timedDialogue);
            DialoguePanel.SetActive(false);

            TurnManager.DialogueInProgress = false;
            playerinDialogue.lastInputWasYes = false;
        }
        else
        {
            DialogueButtonPanel.SetActive(true);
            DialogueText.text = String.Format("You found a star! Want to buy it for  20 coins? You currently have {0} coins!", playerCoins);

            ////TEST, loop turn without player input
            //TriggerYes();
            if (TurnManager.AutomaticTestMode || AI)
            {
                TriggerYes();
            }
        }
    }



    public void TriggerYes()
    {
        TurnManager.DialogueInProgress = false;
        TurnManager.BranchDialogueInProgress = false;

        DialoguePanel.SetActive(false);
        BranchUI.SetActive(false);
        playerinDialogue.lastInputWasYes = true;
    }

    public void TriggerNo()
    {
        TurnManager.DialogueInProgress = false;
        TurnManager.BranchDialogueInProgress = false;
        DialoguePanel.SetActive(false);
        
        BranchUI.SetActive(false);
        playerinDialogue.lastInputWasYes = false;
    }

}
