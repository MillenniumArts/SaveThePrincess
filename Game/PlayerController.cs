using UnityEngine;
using System.Collections;


public class PlayerController: MonoBehaviour {

	#region Variables
	/// <summary>
	/// Reference to the CreateCombination that is attached to this player
	/// </summary>

	public CreateCombination body;


	#region STATS

	/// <summary>
	/// The Maximum health for this player
	/// </summary>

	public int totalHealth ;

	/// <summary>
	/// The Remaining health for this player
	/// </summary>

	public  int remainingHealth;

	#endregion STATS
	#endregion Variables

	#region Public functions

	/// <summary>
	/// Used to pass a custom combination to this script from an outside source.
	/// </summary>
	/// <param name="incomingDamage">int Incoming damage applied to the player</param>
	public void TakeDamage(int incomingDamage){
		this.remainingHealth -= incomingDamage;
	}

	#endregion Public functions

	#region Private functions
	#endregion Private functions

	#region MonoBehaviour 
	/// <summary>
	///Initialize the player with values	
	/// </summary>
	void Start(){
		this.body = this.gameObject.GetComponent<CreateCombination> ();
		this.totalHealth = 100;
		this.remainingHealth = totalHealth;
	}

	#endregion MonoBehaviour
}

