using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour {

    public GameObject DialoguePanel;
    public GameObject DialogueButtonPanel;

    public GameObject BranchUI;

    public Text DialogueText;
    public Player playerinDialogue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerDialogue(string dialogue, Player player)
    {
        playerinDialogue = player;
        if (dialogue == "Star")
        {
            StartCoroutine(TriggerStarDialogue());
        }
        else if(dialogue == "Branch")
        {
            TriggerBranchDialogue();
        }
    }

    private void TriggerBranchDialogue()
    {
        BranchUI.SetActive(true);

        ////TEST, loop turn without player input
        //if (UnityEngine.Random.Range(0, 2) == 0)
        //{
        //    TriggerYes();
        //}
        //else
        //{
        //    TriggerNo();
        //}
    }

    public IEnumerator TriggerStarDialogue()
    {
        DialoguePanel.SetActive(true);


        if(playerinDialogue.playerResources.CoinCount < 20)
        {
            DialogueButtonPanel.SetActive(false);
            DialogueText.text = String.Format("You found a star! You currently have {0} coins, you can't buy it!", playerinDialogue.playerResources.CoinCount);
            yield return new WaitForSeconds(2f);
            DialoguePanel.SetActive(false);

            TurnManager.DialogueInProgress = false;
            playerinDialogue.lastInputWasYes = false;
        }
        else
        {
            DialogueButtonPanel.SetActive(true);
            DialogueText.text = String.Format("You found a star! You currently have {0} coins, want to buy it?", playerinDialogue.playerResources.CoinCount);

            ////TEST, loop turn without player input
            //TriggerYes();
        }
    }



    public void TriggerYes()
    {
        TurnManager.DialogueInProgress = false;
        DialoguePanel.SetActive(false);
        BranchUI.SetActive(false);
        playerinDialogue.lastInputWasYes = true;
    }

    public void TriggerNo()
    {
        TurnManager.DialogueInProgress = false;
        DialoguePanel.SetActive(false);
        BranchUI.SetActive(false);
        playerinDialogue.lastInputWasYes = false;
    }

}
