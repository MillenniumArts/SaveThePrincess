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

		this.TriggerAnimation (this.inventory._items [index].GetItemClass ());
		this.inventory.DisableButtonsIfUsed ();
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
		this.physicalDamage = physicalDamage - damageMod;
		this.playerWeapon.SwapTo(w);							// Swaps all the stats.
		this.playerWeapon.SetCombination(w.GetComponentInChildren<CreateCombination>().GetCurrentComboArray()); // Sets a combination.
		this.playerWeapon.GiveCombination(w.GetItemSubClass());	// Swaps all the sprites to the new weapon.
		this.playerAnimator.SetBool(w.idleAnimParameter, w.idleState);
		this.damageMod = w.GetAtkMod();
		this.physicalDamage = physicalDamage + damageMod;
	}

	/// <summary>
	/// Transfers the purchased armor to the player's body.
	/// </summary>
	/// <param name="a">The armour to be transfered.</param>
	public void TransferPurchasedArmor(Item a){
		this.armor -= armorMod;
		this.playerArmor.SwapTo(a);
		this.armorMod = a.GetDefMod();
		this.armor += armorMod;
		if(this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled == false){
			this.playerArmor.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
	}

	public int GetTotalArmor(){
		return this.armor + this.playerArmor.GetDefMod () + this.playerWeapon.GetDefMod ();
	}

	public int GetTotalDamage(){
		return this.physicalDamage + this.playerArmor.GetAtkMod () + this.playerWeapon.GetAtkMod ();
	}
	#endregion Public functions
	
	#region Private functions
	/// <summary>
	/// Clean up player stats for restart on last tick.
	/// </summary>
	private void DoOnLastTick(){
		this.playerArmor.ClearStats ();
		this.playerWeapon.ClearStats ();
	}

	
	#endregion Private functions
	
	#region MonoBehaviour 
	/// <summary>
	///Initialize the player with values	
	/// </summary>
	void Start(){
		PawnControllerStart();
		this.playerAnimator = GetComponentInChildren<PlayerMoveAnim>().gameObject.GetComponent<Animator>();
		this.dollarBalance = 50;
		this.armorMod = playerArmor.GetDefMod();
		this.armor = armor - armorMod;
		this.armor += armorMod;
		playerArmor.ClearStats();
		playerArmor.itemName = "None";
		armorMod = 0;
	}

	void Update(){
		DoOnFirstTick();
		if (IsDead ()) {
			DoOnLastTick();
		}
	}
	#endregion MonoBehaviour
}

