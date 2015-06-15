using UnityEngine;
using System.Collections;

public class BaseEnemyController : PawnController {

	
	#region Variables
	/// <summary>
	/// The chosen class for this player
	/// </summary>
	public string classType ;
		
	/// <summary>
	/// The number of usable items for this player
	/// </summary>
	public int numUsableItems ;

    public bool isAnimating;
	
	#endregion Variables
	
	#region Public functions

	/// <summary>
	/// Transfers the purchased weapon to the player's hand.
	/// </summary>
	/// <param name="w">The weapon to be transfered.</param>
	public void TransferPurchasedWeapon(Item w){
		this.playerWeapon.SwapTo(w);							// Swaps all the stats.
		this.playerWeapon.SetCombination(w.GetComponentInChildren<CreateCombination>().GetCurrentComboArray()); // Sets a combination.
		this.playerWeapon.GiveCombination(w.GetItemSubClass());	// Swaps all the sprites to the new weapon.
		this.playerAnimator.SetBool(w.idleAnimParameter, w.idleState);
	}
	
	/// <summary>
	/// Transfers the purchased armor to the player's body.
	/// </summary>
	/// <param name="a">The armour to be transfered.</param>
	public void TransferPurchasedArmor(Item a){
		this.playerArmor.SwapTo(a);
		if(this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled == false){
			this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
	}
	
	#endregion Public functions

	#region Protected Functions


	#endregion Protected Functions

	#region Private functions
	
	private void EnemyStart(){
		this.dollarBalance = 25;
	}

	/// <summary>
	/// Drops a random amount of money after enemy dies.
	/// </summary>
	public int DropMoney(){
		return Random.Range (Mathf.FloorToInt(this.dollarBalance/2) , this.dollarBalance);
	}

	#endregion Private functions
	
	#region MonoBehaviour 
	/// <summary>
	///Initialize the Enemy with values	
	/// </summary>
	void Start(){
		EnemyStart();
		PawnControllerStart();
	}

    void Awake()
    {
        isAnimating = false;
    }

	void Update(){
		DoOnFirstTick();
		if (IsDead ()) {
			this.TriggerAnimation("death");
		}
        playerAnimator.SetInteger("Health", remainingHealth);
	}
	#endregion MonoBehaviour


}
