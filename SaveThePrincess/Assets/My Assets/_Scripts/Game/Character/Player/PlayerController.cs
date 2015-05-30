using UnityEngine;
using System.Collections;


public class PlayerController: PawnController {
	
	#region Variables
	/// <summary>
	/// The chosen class for this player
	/// </summary>
	public string classType ;
	
	/// <summary>
	/// The number of usable items for this player
	/// </summary>
	public int numUsableItems ;

	#endregion Variables
	
	#region Public functions

	/// <summary>
	/// Uses the item at specified index.
	/// </summary>
	/// <param name="index">Index.</param>
	public void UseItem(int index){
		if (this.inventory == null) {
			this.inventory = GameObject.FindObjectOfType<InventoryGUIController>();
		}
		this.inventory._items [index].ApplyEffect (this);
		// only use once if potion
		if (this.inventory._items [index].GetItemClass() == "Potion")
			this.inventory._items [index].used = true;
	}

	/// <summary>
	/// Purchases the item.
	/// </summary>
	/// <returns><c>true</c>, if item was purchased, <c>false</c> otherwise.</returns>
	/// <param name="itemCost">Item cost.</param>
	public bool PurchaseItem(int itemCost){
		if (this.dollarBalance - itemCost >= 0) {
			this.dollarBalance -= itemCost;
			return true;
		} else {
			Debug.Log ("Not enough money for that!");
			return false;
		}
	}

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
	
	#region Private functions
	

	
	#endregion Private functions
	
	#region MonoBehaviour 
	/// <summary>
	///Initialize the player with values	
	/// </summary>
	void Start(){
		this.body = GameObject.FindWithTag (this.tag).GetComponentInChildren<CreateCombination> ();
		this.playerAnimator = GetComponentInChildren<Animator>();
		this.spawnWithWeapon = false;
		// initialize weapon if player is supposed to have one
		CallSetWeapon("Sword");
		CallSetArmor("Armor");
		if (!spawnWithWeapon) {			//
			this.weaponComboScript.AllOff ();	// Creates a weapon, sets it to the player's hand and makes it invisible.
			this.weaponComboScript.SwapNow ();
			this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}

		this.dollarBalance = 25;
	}
	#endregion MonoBehaviour
}

