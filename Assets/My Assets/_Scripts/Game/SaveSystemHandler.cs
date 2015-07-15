using UnityEngine;
using System.Collections;

public class SaveSystemHandler : MonoBehaviour
{
    public PlayerController player;

    #region Singleton
    private static SaveSystemHandler _instance;

    public static SaveSystemHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SaveSystemHandler>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }
    #endregion Singleton

    #region Loading

    // TODO: PARSE STAT STRING AFTER CONSTRUCTION

    public void LoadGame()
    {
        // if there is a game to load
        if (PlayerPrefs.GetInt("GameToLoad") == 1)
        {
            // load player data

            string gameData = PlayerPrefs.GetString("GameInfo");

        }
        else
        {
            Debug.Log("NO GAME TO BE LOADED");
        }

    }

    #endregion Loading

    #region Saving
    public void SaveGame()
    {
        if (this.player != null)
        {
            string statString = "";
            // generate a CSV string for stats
            // FORMAT: STAT:###;
            // strip on semi colons first, then colons
            // get player stats
            statString += this.player.GetStatString();
    
            // gameStats

            // place in level (10/15 battles)
            statString += "PLVL" + BattleCounter.GetInstance().GetCurrentBattleCount() + ";";
            // Level
            statString += "LVL:" + BattleCounter.GetInstance().GetBattlesNeeded() + ";";
            // Number Enemies Killed
            statString += "NEK:" + PlayerPrefs.GetInt("score") + ";";

            // get sprite info
            Debug.Log(statString);
            // set flag for game to load next time
            PlayerPrefs.SetInt("GameToLoad", 1);
        }

    }

    #endregion Saving


    // Use this for initialization
	void Start () {
        this.player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.player == null)
        {
            this.player = FindObjectOfType<PlayerController>();
        }
	}
}
