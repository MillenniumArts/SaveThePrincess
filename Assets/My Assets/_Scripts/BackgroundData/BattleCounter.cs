using UnityEngine;
using System.Collections;

public class BattleCounter {

	private static BattleCounter instance = null;	// Reference to this instance of the class.

    public int currentBattleCount;
    public int battlesNeeded;

	public static BattleCounter GetInstance(){
		// Makes sure that this is the only instance of this class.
		if(instance == null){
			instance = new BattleCounter();
		}
		return instance;
	}
    #region getters
    public int GetRemainingBattles()
    {
        return battlesNeeded - currentBattleCount;
    }

    public int GetCurrentBattleCount()
    {
        return currentBattleCount;
    }

    public int GetBattlesNeeded()
    {
        return this.battlesNeeded;
    }
    #endregion getters

    public void IncreaseCurrentBattleCount()
    {
        this.currentBattleCount++;
    }

    public void ResetCurrentBattleCount()
    {
        this.currentBattleCount = 0;
    }

    public void ResetBattlesNeeded()
    {
        this.battlesNeeded = 0;
    }
    #region setters
    public void SetBattlesNeeded(int amount)
    {
        this.battlesNeeded = amount;
    }

    public void SetCurrentBattleCount(int c)
    {
        this.currentBattleCount = c;
    }

    #endregion setters

    private BattleCounter() { }


}
