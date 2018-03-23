using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreenHandler : MonoBehaviour {

    [Header("UI elements")]
    public Text Rank1Text;
    public Text Rank2Text;
    public Text Rank3Text;
    public Text Rank4Text;

    [Header("Locations")]
    public GameObject Rank1Loc;
    public GameObject Rank2Loc;
    public GameObject Rank3Loc;
    public GameObject Rank4Loc;

    [Header("Players")]
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    void Awake()
    {
        
    }

    public void btnToMainMenu_onclick()
    {
        SceneManager.LoadScene(0);
    }
}
