using UnityEngine;
using System.Collections;

public class EnemyStats {
#region Singleton
    private static EnemyStats instance = null;	// Reference to this instance of the class.

    public static EnemyStats GetInstance()
    {
        // Makes sure that this is the only instance of this class.
        if (instance == null)
        {
            instance = new EnemyStats();
        }
        return instance;
    }

    private EnemyStats() { }
#endregion Singleton

#region Variables
    /// <summary>
    /// Previous Enemy's Stats.
    /// </summary>
    private int previousEnemyHP, previousEnemyNRG, previousEnemyATK, previousEnemyDEF;
    /// <summary>
    /// Current Enemy's Stats.
    /// </summary>
    private int currentEnemyHP, currentEnemyNRG, currentEnemyATK, currentEnemyDEF;
    /// <summary>
    /// Enemy Stat Checkpoint.
    /// </summary>
    private int lastCheckpointHP, lastCheckpointNRG, lastCheckpointATK, lastCheckpointDEF;
    /// <summary>
    /// Minimum stat increase %.
    /// </summary>
    private float MIN_inc = 0f;
    /// <summary>
    /// Maximum stat increase %.
    /// </summary>
    private float MAX_inc = 0.05f;
    /// <summary>
    /// Minimum stat increase % for "boss" enemies.
    /// </summary>
    private float MIN_inc_boss = 0.1f;
    /// <summary>
    /// Maximum stat increase % for "boss" enemies.
    /// </summary>
    private float MAX_inc_boss = 0.15f;
    /// <summary>
    /// Minimum stat increase % for filling the stat gap between the player's and the enemy's HP and NRG.
    /// </summary>
    private float MIN_gapFill_HP_NRG = 0.50f;
    /// <summary>
    /// Maximum stat increase % for filling the stat gap between the player's and the enemy's HP and NRG.
    /// </summary>
    private float MAX_gapFill_HP_NRG = 0.65f;
    /// <summary>
    /// Minimum stat increase % for filling the stat gap between the player's and the enemy's ATK and DEF.
    /// </summary>
    private float MIN_gapFill_ATK_DEF = 0.50f;
    /// <summary>
    /// Maximum stat increase % for filling the stat gap between the player's and the enemy's ATK and DEF.
    /// </summary>
    private float MAX_gapFill_ATK_DEF = 0.65f;
    /// <summary>
    /// Are we on the very first enemy?
    /// </summary>
    private bool firstEnemy = true;
    /// <summary>
    /// Are we at the beginning of a run of enemies.
    /// </summary>
    private bool startOfRun = true;
#endregion Variables

#region Public Functions
    #region Getters
    /// <summary>
    /// Sets the stats of an enemy that is passed through.
    /// </summary>
    /// <param name="enemy">BaseEnemyController class.</param>
    public void GetEnemyBaseStats(BaseEnemyController enemy, PlayerController player)
    {
       // Debug.Log("Checkpoint is: " + lastCheckpointHP + " HP, " + lastCheckpointNRG + " NRG, " + lastCheckpointATK + " ATK, " + lastCheckpointDEF + " DEF.");
       // Debug.Log("Previous is: " + previousEnemyHP + " HP, " + previousEnemyNRG + " NRG, " + previousEnemyATK + " ATK, " + previousEnemyDEF + " DEF.");
       // Debug.Log("Start of Run bool = " + startOfRun);
        if (startOfRun == true)
        {
            //SetCheckpoint();
            startOfRun = false;
        }
        //Debug.Log("Getting Enemy Stats");
        float min;
        float max;
        if (!firstEnemy)
        {
            // Check the HP and NRG.
            if (CheckStatGap(previousEnemyHP, previousEnemyNRG, player.totalHealth, player.totalEnergy, MIN_gapFill_HP_NRG, MAX_gapFill_HP_NRG) == false)
            {
                Debug.Log("Not first enemy.");
                if (!CheckForBoss())
                {
                    Debug.Log("Not boss.");
                    min = MIN_inc;
                    max = MAX_inc;
                }
                else
                {
                    Debug.Log("Enemy is boss.");
                    min = MIN_inc_boss;
                    max = MAX_inc_boss;
                }
            }
            else
            {
                Debug.Log("Stat gap fill HP and NRG");
                min = MIN_gapFill_HP_NRG;
                max = MAX_gapFill_HP_NRG;
            }
            Debug.Log("Increasing HP and NRG stats");
            currentEnemyHP = previousEnemyHP + RandomIncrease(previousEnemyHP, min, max);
            currentEnemyNRG = previousEnemyNRG + RandomIncrease(previousEnemyNRG, min, max);

            // Check the ATK and DEF.
            if (CheckStatGap(previousEnemyATK, previousEnemyDEF, player.physicalDamage, player.armor, MIN_gapFill_ATK_DEF, MAX_gapFill_ATK_DEF) == false)
            {
                Debug.Log("Not first enemy.");
                if (!CheckForBoss())
                {
                    Debug.Log("Not boss.");
                    min = MIN_inc;
                    max = MAX_inc;
                }
                else
                {
                    Debug.Log("Enemy is boss.");
                    min = MIN_inc_boss;
                    max = MAX_inc_boss;
                }
            }
            else
            {
                Debug.Log("Stat gap fill ATK and DEF");
                min = MIN_gapFill_ATK_DEF;
                max = MAX_gapFill_ATK_DEF;
            }
            Debug.Log("Increasing ATK and DEF stats");
            currentEnemyATK = previousEnemyATK + RandomIncrease(previousEnemyATK, min, max);
            currentEnemyDEF = previousEnemyDEF + RandomIncrease(previousEnemyDEF, min, max);
        }

        enemy.SetStats(currentEnemyHP, currentEnemyNRG, currentEnemyATK, currentEnemyDEF);
        StatFlip();
    }

    /// <summary>
    /// Returns the firstEnemy bool.
    /// </summary>
    /// <returns>firstEnemy bool</returns>
    public bool GetFirstEnemyBool()
    {
        return firstEnemy;
    }

    public int GetCurrentEnemyATK()
    {
        return this.currentEnemyATK;
    }

    public int GetCurrentEnemyDEF()
    {
        return this.currentEnemyDEF;
    }

    public int GetCurrentEnemyNRG()
    {
        return this.currentEnemyNRG;
    }

    public int GetCurrentEnemyHP()
    {
        return this.currentEnemyHP;
    }
    #endregion Getters

    #region Setters
    /// <summary>
    /// Set the base stat for the enemies in the current playthrough of the game.  ****These are the stats for the first enemy.****
    /// </summary>
    /// <param name="HP">Hit Points</param>
    /// <param name="EN">Energy Points</param>
    /// <param name="ATK">Attack Stat</param>
    /// <param name="DEF">Defence Stat</param>
    public void SetEnemyBaseStats(int HP, int EN, int ATK, int DEF)
    {
        // Set the base stats.
        previousEnemyHP = HP;
        previousEnemyNRG = EN;
        previousEnemyATK = ATK;
        previousEnemyDEF = DEF;
        // Increase the base stats for the first enemy.
        currentEnemyHP = previousEnemyHP + RandomIncrease(previousEnemyHP, 0f, 0.1f);
        currentEnemyNRG = previousEnemyNRG + RandomIncrease(previousEnemyNRG, 0f, 0.1f);
        currentEnemyATK = previousEnemyATK + RandomIncrease(previousEnemyATK, 0f, 0.1f);
        currentEnemyDEF = previousEnemyDEF + RandomIncrease(previousEnemyDEF, 0f, 0.1f);
        SetCheckpoint();
        StatFlip();
    }

    /// <summary>
    /// Sets the startOfRun bool.
    /// </summary>
    /// <param name="tog">bool</param>
    public void SetStartOfRun(bool tog)
    {
        this.startOfRun = tog;
    }

    /// <summary>
    /// Sets the firstEnemy bool.
    /// </summary>
    /// <param name="tog">bool</param>
    public void SetVeryFirstEnemy(bool tog)
    {
        this.firstEnemy = tog;
    }
    #endregion Setters

    #region Resetters
    /// <summary>
    /// Reset the enemy base stats to 0.
    /// </summary>
    public void ResetEnemyBaseStats()
    {
        previousEnemyHP = 0;
        previousEnemyNRG = 0;
        previousEnemyATK = 0;
        previousEnemyATK = 0;
        firstEnemy = true; // When the game is reset the firstEnemy bool is reset to true.
        startOfRun = true; // Reset startOfRun bool to true as well.
    }

    /// <summary>
    /// Sets the checkpoint of the enemy stat creation base stats to the current enemy's stats.
    /// </summary>
    public void SetCheckpoint()
    {
        //Debug.Log("SetCheckpoint");
        lastCheckpointHP = currentEnemyHP;
        lastCheckpointNRG = currentEnemyNRG;
        lastCheckpointATK = currentEnemyATK;
        lastCheckpointDEF = currentEnemyDEF;
    }

    /// <summary>
    /// Restores the enemy stats creation base stats to the previous checkpoints stats.
    /// </summary>
    public void ResetCheckpoint()
    {
        startOfRun = true;
        previousEnemyHP = lastCheckpointHP;
        previousEnemyNRG = lastCheckpointNRG;
        previousEnemyATK = lastCheckpointATK;
        previousEnemyDEF = lastCheckpointDEF;
    }
    #endregion Resetters
#endregion Public Functions

#region Private Functions
    /// <summary>
    /// Sets the current enemy stats to the previous enemy stats.
    /// </summary>
    private void StatFlip()
    {
        previousEnemyHP = currentEnemyHP;
        previousEnemyNRG = currentEnemyNRG;
        previousEnemyATK = currentEnemyATK;
        previousEnemyDEF = currentEnemyDEF;
    }

    /// <summary>
    /// Returns an increase for the stat passed into it by a random amount between the min and max passed in.
    /// </summary>
    /// <param name="stat">Stat to get an increase for.</param>
    /// <param name="min">Minimum for random range.</param>
    /// <param name="max">Maximum for random range.</param>
    /// <returns></returns>
    private int RandomIncrease(int stat, float min, float max)
    {
        int increase = (int)(stat * (Random.Range(min, max)));
        if (increase < 1)
        {
            increase = Random.Range(1, 6);
        }
        return increase;
    }

    /// <summary>
    /// Checks to see if the player is at a "boss" enemy.
    /// </summary>
    private bool CheckForBoss()
    {
        return BattleCounter.GetInstance().battlesNeeded == 1;
    }

    /*private bool CheckStatGap(PlayerController _player)
    {
        float tempHP = (float)previousEnemyHP / (float)_player.totalHealth;
        //Debug.Log(previousEnemyHP + " / " + _player.totalHealth + " HP Gap is " + tempHP + "%");
        float tempNRG = (float)previousEnemyNRG / (float)_player.totalEnergy;
        //Debug.Log(previousEnemyNRG + " / " + _player.totalEnergy + " NRG Gap is " + tempNRG + "%");
        float tempATK = (float)previousEnemyATK / (float)_player.physicalDamage;
        //Debug.Log(previousEnemyATK + " / " + _player.physicalDamage + " ATK Gap is " + tempATK + "%");
        float tempDEF = (float)previousEnemyDEF / (float)_player.armor;
        //Debug.Log(previousEnemyDEF + " / " + _player.armor + " DEF Gap is " + tempDEF + "%");

        float tempStatAvg_HP_NRG = (tempHP + tempNRG) / 2;
        float tempStatAvg_ATK_DEF = (tempATK + tempDEF) / 2;

        Debug.Log("HP and Energy stat Gap is " + tempStatAvg_HP_NRG * 100f + "%");
        Debug.Log("ATK and DEF stat Gap is " + tempStatAvg_ATK_DEF * 100f + "%");

        if (tempStatAvg_HP_NRG <= 0.15f)
        {
            MIN_gapFill_HP_NRG = 2.5f;
            MAX_gapFill_HP_NRG = 3f;
            return true;
        }
        else if (tempStatAvg_HP_NRG <= 0.25f)
        {
            MIN_gapFill_HP_NRG = 1f;
            MAX_gapFill_HP_NRG = 1.05f;
            return true;
        }
        else if (tempStatAvg_HP_NRG <= 0.4f)
        {
            MIN_gapFill_HP_NRG = 0.75f;
            MAX_gapFill_HP_NRG = 0.85f;
            return true;
        }
        else if (tempStatAvg_HP_NRG <= 0.55f)
        {
            MIN_gapFill_HP_NRG = 0.45f;
            MAX_gapFill_HP_NRG = 0.55f;
            return true;
        }
        if (tempStatAvg_HP_NRG <= 0.7f)
        {
            MIN_gapFill_HP_NRG = 0.25f;
            MAX_gapFill_HP_NRG = 0.35f;
            return true;
        }
        else
        {
            return false;
        }
    }*/
    
    /// <summary>
    /// Checks the stat gap (enemy/player) of the stats passeed in.
    /// </summary>
    /// <param name="eStat1">First enemy's stat. (HP or ATK)</param>
    /// <param name="eStat2">Second enemy's stat. (NRG or DEF)</param>
    /// <param name="pStat1">First player's stat. (HP or ATK)</param>
    /// <param name="pStat2">Second player's stat. (HP or ATK)</param>
    /// <param name="min">The MIN variable to pass the new minimum to.</param>
    /// <param name="max">The MAX variable to pass the new maximum to.</param>
    /// <returns></returns>
    private bool CheckStatGap(int eStat1, int eStat2, int pStat1, int pStat2, float min, float max)
    {
        float tempStatAvg = (float)(eStat1 + eStat2) / (float)(pStat1+pStat2);
        //Debug.Log(eStat1 +  " + " + eStat2 + " / " + pStat1 + " + " + pStat2 + " = " + tempStatAvg);

        Debug.Log("The stat Gap is " + tempStatAvg);

        if (tempStatAvg <= 0.15f)
        {
            min = 2.5f;
            max = 3f;
            return true;
        }
        else if (tempStatAvg <= 0.25f)
        {
            min = 1f;
            max = 1.05f;
            return true;
        }
        else if (tempStatAvg <= 0.4f)
        {
            min = 0.75f;
            max = 0.85f;
            return true;
        }
        else if (tempStatAvg <= 0.55f)
        {
            min = 0.45f;
            max = 0.55f;
            return true;
        }
        if (tempStatAvg <= 0.7f)
        {
            min = 0.25f;
            max = 0.35f;
            return true;
        }
        else
        {
            return false;
        }
    }

#endregion Private Functions
}
