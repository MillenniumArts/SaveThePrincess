using UnityEngine;
using System.Collections;

public class DifficultyLevel{

	private static DifficultyLevel instance = null;	// Reference to this instance of the class.
	
	public static DifficultyLevel GetInstance(){
		// Makes sure that this is the only instance of this class.
		if(instance == null){
			instance = new DifficultyLevel();
		}
		return instance;
	}

	private DifficultyLevel(){}	// Prevents this class from being instantiated by something else.

	private int difficulty = 1;

	public int DifficultyMultiplier(){
		return difficulty;
	}

	// Call after each battle.
	public void IncreaseDifficulty(){
		difficulty += 1;
	}

	// Call after death.
	public void ResetDifficulty(){
		difficulty = 1;
	}
}
