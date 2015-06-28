using UnityEngine;
using System.Collections;

public class EnemyStats {
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

    private int previousEnemyHP, previousEnemyEN, previousEnemyATK, previousEnemyDEF;
    private int currentEnemyHP, currentEnemyEN, currentEnemyATK, currentEnemyDEF;
    private bool firstEnemy = true;
    private int firstCount = 0;
    private int getCount = 0;

    /// <summary>
    /// Set the base stat for the enemies in the current playthrough of the game.
    /// </summary>
    /// <param name="HP">Hit Points</param>
    /// <param name="EN">Energy Points</param>
    /// <param name="ATK">Attack Stat</param>
    /// <param name="DEF">Defence Stat</param>
    public void SetEnemyBaseStats(int HP, int EN, int ATK, int DEF)
    {
        // Set the base stats.
        previousEnemyHP = HP;
        previousEnemyEN = EN;
        previousEnemyATK = ATK;
        previousEnemyDEF = DEF;
        // Increase the base stats for the first enemy.
        currentEnemyHP = previousEnemyHP + RandomIncrease(previousEnemyHP, 0f, 0.1f);
        currentEnemyEN = previousEnemyEN + RandomIncrease(previousEnemyEN, 0f, 0.1f);
        currentEnemyATK = previousEnemyATK + RandomIncrease(previousEnemyATK, 0f, 0.1f);
        currentEnemyDEF = previousEnemyDEF + RandomIncrease(previousEnemyDEF, 0f, 0.1f);
        StatFlip();
    }
    /// <summary>
    /// Reset the enemy base stats to 0.
    /// </summary>
    public void ResetEnemyBaseStats()
    {
        previousEnemyHP = 0;
        previousEnemyEN = 0;
        previousEnemyATK = 0;
        previousEnemyATK = 0;
        firstEnemy = true; // When the game is reset the firstEnemy bool is reset to true.
        firstCount = 0;
    }

    /// <summary>
    /// Get New Enemy HP.
    /// </summary>
    /// <returns>HP</returns>
    public int GetNewEnemyHP()
    {
        if (!firstEnemy){
            if (!CheckForBoss()){
                currentEnemyHP = previousEnemyHP + RandomIncrease(previousEnemyHP, 0f, 0.05f);   // If the enemy is a regular enemy.
            }else{
                currentEnemyHP = previousEnemyHP + RandomIncrease(previousEnemyHP, 0.1f, 0.15f);  // If the enemy is a "boss" enemy.
            }
        }else{
            firstCount++;
            if (firstCount == 4){
                firstEnemy = false;
            }
        }
        getCount++;
        if (getCount == 4){
            StatFlip();
            getCount = 0;
        }
        return currentEnemyHP;
    }
    /// <summary>
    /// Get New Enemy Energy stat.
    /// </summary>
    /// <returns>Energy.</returns>
    public int GetNewEnemyEN()
    {
        if (!firstEnemy)
        {
            if (!CheckForBoss())
            {
                currentEnemyEN = previousEnemyEN + RandomIncrease(previousEnemyEN, 0f, 0.05f);   // If the enemy is a regular enemy.
            }
            else
            {
                currentEnemyEN = previousEnemyEN + RandomIncrease(previousEnemyEN, 0.1f, 0.15f);  // If the enemy is a "boss" enemy.
            }
        }
        else
        {
            firstCount++;
            if (firstCount == 4)
            {
                firstEnemy = false;
            }
        }
        getCount++;
        if (getCount == 4)
        {
            StatFlip();
            getCount = 0;
        }
        return currentEnemyEN;
    }
    /// <summary>
    /// Gets new enemy Attack stat.
    /// </summary>
    /// <returns>Attack stat.</returns>
    public int GetNewEnemyATK()
    {
        if (!firstEnemy)
        {
            if (!CheckForBoss())
            {
                currentEnemyATK = previousEnemyATK + RandomIncrease(previousEnemyATK, 0f, 0.05f);   // If the enemy is a regular enemy.
            }
            else
            {
                currentEnemyATK = previousEnemyATK + RandomIncrease(previousEnemyATK, 0.1f, 0.15f);  // If the enemy is a "boss" enemy.
            }
        }
        else
        {
            firstCount++;
            if (firstCount == 4)
            {
                firstEnemy = false;
            }
        }
        getCount++;
        if (getCount == 4)
        {
            StatFlip();
            getCount = 0;
        }
        return currentEnemyATK;
    }
    /// <summary>
    /// Gets new enemy Defence stat.
    /// </summary>
    /// <returns></returns>
    public int GetNewEnemyDEF()
    {
        if (!firstEnemy)
        {
            if (!CheckForBoss())
            {
                currentEnemyDEF = previousEnemyDEF + RandomIncrease(previousEnemyDEF, 0f, 0.05f);   // If the enemy is a regular enemy.
            }
            else
            {
                currentEnemyDEF = previousEnemyDEF + RandomIncrease(previousEnemyDEF, 0.1f, 0.15f);  // If the enemy is a "boss" enemy.
            }
        }
        else
        {
            firstCount++;
            if (firstCount == 4)
            {
                firstEnemy = false;
            }
        }
        getCount++;
        if (getCount == 4)
        {
            StatFlip();
            getCount = 0;
        }
        return currentEnemyDEF;
    }

    /// <summary>
    /// Sets the current enemy stats to the previous enemy stats.
    /// </summary>
    private void StatFlip()
    {
        previousEnemyHP = currentEnemyHP;
        previousEnemyEN = currentEnemyEN;
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
            increase = Random.Range(1, 4);
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
}
