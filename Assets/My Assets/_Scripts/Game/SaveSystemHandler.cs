using UnityEngine;
using System.Collections;
using System;

public class SaveSystemHandler : MonoBehaviour
{
    public PlayerController player;
    public BaseEnemyController enemy;
    public bool inBattle;

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

    // inB:0;PLVL:0;LVL:1;NEK:0;rHP:100;tHP:100;rNRG:100;tNRG:100;DMG:30;ARM:10;wDMG:3;wARM:0;aDMG:0;aARM:0;eHP:0;eNRG:0;eDMG:0;eARM:0;eHP:0;eNRG:0;eDMG:0;eARM:0;
    public void LoadGame()
    {
        // if there is a game to load
        if (PlayerPrefs.GetInt("GameToLoad") == 1)
        {
            Debug.Log("Loading game data...");
            // load player data
            string gameData = PlayerPrefs.GetString("GameData");
            Debug.Log(gameData);

            char[] delim0 = { '_' };
            char[] delim1 = { ';' };
            char[] delim2 = { ':' };

            string[] data0 = gameData.Split(delim0);    // split on '_' to split game/player/LF/cP, gives groups of stats

            //for (int i = 0; i < data0.Length; i++ )
            //  Debug.Log(data0[i] + " ");

            string[] gameStats = data0[0].Split(delim1);        // gives STAT:### on each index
            string[] playerStats = data0[1].Split(delim1);      // gives STAT:### on each index
            string[] lastFoughtStats = data0[2].Split(delim1);  // gives STAT:### on each index
            string[] checkpointStats = data0[3].Split(delim1);  // gives STAT:### on each index

            int thp = 0, tnrg = 0, rhp = 0, rnrg = 0, dmg = 0, arm = 0;

            for (int i = 0; i < data0.Length; i++)
            {
                switch (i)
                {
                    // game data on data0[0];
                    case 0:
                        Debug.Log("Loading Game info...");
                        for (int j = 0; j < gameStats.Length; j++)
                        {
                            string[] statSet = gameStats[j].Split(delim2); // Gives [0]:STAT [1]: ###

                            switch (statSet[0])
                            {
                                case "inB":
                                    // in Battle?
                                    if (Convert.ToInt32(statSet[1]) == 1)
                                    {
                                        this.inBattle = true;
                                    }
                                    else
                                    {
                                        this.inBattle = false;
                                    }
                                    break;
                                case "PLVL":
                                    // current battle no
                                    BattleCounter.GetInstance().SetCurrentBattleCount(Convert.ToInt32(statSet[1]));
                                    break;
                                case "LVL":
                                    // remaining battles
                                    BattleCounter.GetInstance().SetBattlesNeeded(Convert.ToInt32(statSet[1]));
                                    break;
                                case "NEK":
                                    // num enemies killed
                                    PlayerPrefs.SetInt("score", Convert.ToInt32(statSet[1]));
                                    break;
                                case "HTL":
                                    // Healing turns left
                                    this.player.numTurnsLeftToHeal = Convert.ToInt32(statSet[1]);
                                    break;

                            }
                        }
                        break;
                    // player data on data0[1];
                    case 1:
                        Debug.Log("Loading Player info...");
                        for (int j = 0; j < playerStats.Length; j++)
                        {
                            string[] statSet = playerStats[j].Split(delim2);// Gives [0]:STAT [1]: ###

                            //Debug.Log(playerStats[j]);
                            switch (statSet[0])
                            {
                                case "rHP":
                                    // rem HP
                                    this.player.remainingHealth = Convert.ToInt32(statSet[1]);
                                    break;
                                case "tHP":
                                    // total HP
                                    this.player.totalHealth = Convert.ToInt32(statSet[1]);
                                    break;
                                case "rNRG":
                                    // rem NRG
                                    this.player.remainingEnergy = Convert.ToInt32(statSet[1]);
                                    break;
                                case "tNRG":
                                    // total NRG
                                    this.player.totalEnergy = Convert.ToInt32(statSet[1]);
                                    break;
                                /*case "Magic"
                                 * // magical damage
                                 * this.player.magicalDamage= Convert.ToInt32(statSet[1]);
                                 * break; */
                                case "DMG":
                                    // base physical damage
                                    this.player.physicalDamage = Convert.ToInt32(statSet[1]);
                                    break;
                                case "ARM":
                                    // base armor
                                    this.player.armor = Convert.ToInt32(statSet[1]);
                                    break;
                                case "wDMG":
                                    // weapon damage stat
                                    this.player.playerWeapon.SetDamage(Convert.ToInt32(statSet[1]));
                                    break;
                                case "wARM":
                                    // weapon armor stat
                                    this.player.playerWeapon.SetArmor(Convert.ToInt32(statSet[1]));
                                    break;
                                case "aDMG":
                                    //armor damage stat
                                    this.player.playerArmor.SetDamage(Convert.ToInt32(statSet[1]));
                                    break;
                                case "aARM":
                                    // armor armor stat
                                    this.player.playerArmor.SetArmor(Convert.ToInt32(statSet[1]));
                                    break;
                                case "iA":
                                    // inventory: apples
                                    this.player.inventory.Apples = Convert.ToInt32(statSet[1]);
                                    break;
                                case "iB":
                                    // inventory: Bread
                                    this.player.inventory.Bread = Convert.ToInt32(statSet[1]);
                                    break;
                                case "iC":
                                    // inventory: Cheese
                                    this.player.inventory.Cheese = Convert.ToInt32(statSet[1]);
                                    break;
                                case "iH":
                                    // inventory: HPots
                                    this.player.inventory.HealthPotions = Convert.ToInt32(statSet[1]);
                                    break;
                                case "iE":
                                    // inventory: EPots
                                    this.player.inventory.EnergyPotions = Convert.ToInt32(statSet[1]);
                                    break;
                            }
                        }

                        break;
                    // last fought on data0[2];
                    case 2:
                        Debug.Log("Loading Enemy info...");
                        for (int j = 0; j < lastFoughtStats.Length; j++)
                        {
                            string[] statSet = lastFoughtStats[j].Split(delim2);// Gives [0]:STAT [1]: ###
                            switch (statSet[0])
                            {
                                case "etHP": thp = Convert.ToInt32(statSet[1]);
                                    break;
                                case "erHP": rhp = Convert.ToInt32(statSet[1]);
                                    break;
                                case "etNRG": tnrg = Convert.ToInt32(statSet[1]);
                                    break;
                                case "erNRG": rnrg = Convert.ToInt32(statSet[1]);
                                    break;
                                case "eDMG": dmg = Convert.ToInt32(statSet[1]);
                                    break;
                                case "eARM": arm = Convert.ToInt32(statSet[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        // Recreates enemy
                        EnemyStats.GetInstance().LoadNewEnemy(thp, rhp, rnrg, tnrg, dmg, arm);
                        break;
                    // check point on data0[3];
                    case 3:
                        Debug.Log("Loading CheckPoint info...");
                        for (int j = 0; j < checkpointStats.Length; j++)
                        {
                            string[] statSet = lastFoughtStats[j].Split(delim2);// Gives [0]:STAT [1]: ###
                            switch (statSet[0])
                            {
                                case "eHP": thp = Convert.ToInt32(statSet[1]);
                                    break;
                                case "eNRG": tnrg = Convert.ToInt32(statSet[1]);
                                    break;
                                case "eDMG": dmg = Convert.ToInt32(statSet[1]);
                                    break;
                                case "eARM": arm = Convert.ToInt32(statSet[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        EnemyStats.GetInstance().SetCheckpointData(thp, tnrg, dmg, arm);
                        break;
                    case 4:
                        // Sprites
                        
                        // LoadSprites();

                    
                        break;
                    default:
                        break;
                }
            }
            Debug.Log("Load Complete");
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

            // TODO: PLAYER INVENTORY!!***
            //       HEAL TURNS

            /* EnemyStats*/
            /* Last Fought Enemy
             * eHP
             * eNRG
             * eDMG
             * eARM
             */
            /* Checkpoint Enemy
             * eHP
             * eNRG
             * eDMG
             * eARM
             */

            if (this.player.inBattle){
                statString += "inB:" + 1 + ";";
                EnemyStats.GetInstance().SetLastFoughtEnemyStatString(this.enemy.GetEnemyStatString());
            }
            else
            {
                statString += "inB:" + 0 + ";";
                EnemyStats.GetInstance().SetLastFoughtEnemyStatString(this.player.GetEnemyStatString());
            }

            // current battle number(10/15 battles)
            statString += "PLVL:" + BattleCounter.GetInstance().GetCurrentBattleCount() + ";";
            // Level (total number of battles needed)
            statString += "LVL:" + BattleCounter.GetInstance().GetBattlesNeeded() + ";";
            // Number Enemies Killed
            statString += "NEK:" + PlayerPrefs.GetInt("score") + ";";
            // Healing Turns Left
            statString += "HTL:" + this.player.numTurnsLeftToHeal +  ";";

        // BREAK
        statString += "_";

            // get player stats
            statString += this.player.GetPlayerStatString();

            // Inventory
            statString += "iA:" + this.player.inventory.Apples + ";";
            statString += "iB:" + this.player.inventory.Bread + ";";
            statString += "iC:" + this.player.inventory.Cheese + ";";
            statString += "iH:" + this.player.inventory.HealthPotions + ";";
            statString += "iE:" + this.player.inventory.EnergyPotions+ ";";

        // BREAK
        statString += "_";

            // get enemy stats
            // SAVE LAST FOUGHT ENEMY stats
            
            // Last fought
            statString += EnemyStats.GetInstance().GetLastFoughtEnemyStatString();

        // BREAK
        statString += "_";

            // Checkpoint
            if (EnemyStats.GetInstance().GetCheckpointEnemyString() == "")
            {
                // if no check point use player stats against himself
                EnemyStats.GetInstance().SetCheckpointEnemyStatString(this.player.GetEnemyStatString());
            }
            statString += EnemyStats.GetInstance().GetCheckpointEnemyString();
            // BREAK
            statString += "_";

            
            // get sprite info
            
            // statString += SaveSprites();


            // Save to PlayerPrefs
            PlayerPrefs.SetString("GameData", statString);



            Debug.Log(statString);
            // set flag for game to load next time
            PlayerPrefs.SetInt("GameToLoad", 1);
        }
        
    }

    #endregion Saving

    void GetPlayerEnemy()
    {
        this.player = FindObjectOfType<PlayerController>();
        this.enemy = FindObjectOfType<BaseEnemyController>();
    }

    // Use this for initialization
    void Start()
    {
        this.player = FindObjectOfType<PlayerController>();
        this.enemy = FindObjectOfType<BaseEnemyController>();
        if (this.enemy == null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.player == null || this.enemy == null)
        {
            GetPlayerEnemy();
        }
    }
}
