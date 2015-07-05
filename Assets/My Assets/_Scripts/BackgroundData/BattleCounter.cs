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

    public int GetRemainingBattles()
    {
        return battlesNeeded - currentBattleCount;
    }

    public int GetCurrentBattleCount()
    {
        Debug.Log("Current battle count" + currentBattleCount);
        return currentBattleCount;
    }

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

    public void SetBattlesNeeded(int amount)
    {
        this.battlesNeeded = amount;
    }

    private BattleCounter() { }


}
