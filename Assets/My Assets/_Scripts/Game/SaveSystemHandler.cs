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
    // inB:0;PLVL:0;LVL:1;NEK:0;rHP:100;tHP:100;rNRG:100;tNRG:100;DMG:30;ARM:10;wDMG:3;wARM:0;aDMG:0;aARM:0;
    public void LoadGame()
    {
        // if there is a game to load
        if (PlayerPrefs.GetInt("GameToLoad") == 1)
        {

            Debug.Log("Loading game data...");
            // load player data
            string gameData = PlayerPrefs.GetString("GameData");
            Debug.Log(gameData);

            char[] delim1 = { ';' };
            char[] delim2 = { ':' };

            // split the string into parts
            string[] data = gameData.Split(delim1);

            for (int i = 0; i < data.Length; i++)
            {
                // split key/value pairs
                string[] pairs = data[i].Split(delim2);

                Debug.Log(pairs[0]);

                switch (pairs[0])
                {
                    case "inB":
                        // in Battle?
                        break;
                    case "PLVL":
                        // current battle no
                        break;
                    case "LVL":
                        // remaining battles
                        break;
                    case "NEK":
                        // num enemies killed
                        break;
                    case "rHP":
                        // rem HP
                        break;
                    case "tHP":
                        // total HP
                        break;
                    case "rNRG":
                        // rem NRG
                        break;
                    case "tNRG":
                        // total NRG
                        break;
                    /*case "Magic"
                     * // magical damage
                     * break; */
                    case "DMG":
                        // base physical damage
                        break;
                    case "ARM":
                        // base armor
                        break;
                    case "wDMG":
                        // weapon damage stat
                        break;
                    case "wARM":
                        // weapon armor stat
                        break;
                    case "aDMG":
                        //armor damage stat
                        break;
                    case "aARM":
                        // armor armor stat
                        break;
                    case "":
                        // Other
                        break;
                }

            }

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

            // FORMAT:
            /* in battle? (1/0)
             * current battle
             * remaining battles
             * Num Enemies Killed
             * PlayerStats
             */
            /* 
             * remHP
             * totHP
             * remNRG
             * totNRG
             * [Magic]
             * DMG
             * ARM
             * wDMG
             * wARM
             * aDMG
             * aARM
             */


            if (this.player.inBattle)
                statString += "inB:" + 1 + ";";
            else
                statString += "inB:" + 0 + ";";

            // current battle number(10/15 battles)
            statString += "PLVL:" + BattleCounter.GetInstance().GetCurrentBattleCount() + ";";
            // Level (total number of battles needed)
            statString += "LVL:" + BattleCounter.GetInstance().GetBattlesNeeded() + ";";
            // Number Enemies Killed
            statString += "NEK:" + PlayerPrefs.GetInt("score") + ";";

            // get player stats
            statString += this.player.GetStatString();

            // Save to PlayerPrefs
            PlayerPrefs.SetString("GameData", statString);

            // get sprite info
            Debug.Log(statString);
            // set flag for game to load next time
            PlayerPrefs.SetInt("GameToLoad", 1);
        }

    }

    #endregion Saving


    // Use this for initialization
    void Start()
    {
        this.player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.player == null)
        {
            this.player = FindObjectOfType<PlayerController>();
        }
    }
}
