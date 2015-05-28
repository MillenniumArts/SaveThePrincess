using UnityEngine;
using System.Collections;


public class PlayerController: MonoBehaviour {
	
	#region Variables
	/// <summary>
	/// Reference to the CreateCombination that is attached to this player, holds reference to Sprite
	/// </summary>
	public CreateCombination body;

	/// <summary>
	/// The player animator.
	/// </summary>
	public Animator playerAnimator;
	
	/// <summary>
	/// The Maximum Mana for this player
	/// </summary>
	public string playerName ;
	
	/// <summary>
	/// The chosen class for this player
	/// </summary>
	public string classType ;
	
	/// <summary>
	/// The number of armor items for this player
	/// </summary>
	public int numArmorItems ;
	
	/// <summary>
	/// The number of weapon items for this player
	/// </summary>
	public int numWeaponItems ;
	
	/// <summary>
	/// The number of usable items for this player
	/// </summary>
	public int numUsableItems ;
	
	#region Inventory
	///<summary>
	///Player Armor - An array of Item objects that represents all armor objects for the player
	///</summary>
	public Armor playerArmor;
	
	///<summary>
	///Player Weapons - An array of Item objects that represents all weapon objects for the player
	///</summary>
	public Weapon playerWeapon = null;
	
	/// <summary>
	/// The player hand.
	/// </summary>
	public Transform playerHand;
	/// <summary>
	/// whether or not player will spawn with weapon.
	/// </summary>
	public bool spawnWithWeapon = false;

	/// <summary>
	/// The weapon combo script.
	/// </summary>
	public CreateWeaponCombination weaponComboScript;
	
	///<summary>
	///Player Usables - An array of Item objects that represents all usable objects for the player
	///</summary>
	//public Item[] usableItems;
	
	/// <summary>
	/// The inventory of the player.
	/// </summary>
	public InventoryGUIController inventory;
	
	#endregion Inventory
		
	#region STATS
	/// <summary>
	/// The Maximum health for this player
	/// </summary>
	public int totalHealth ;
	
	/// <summary>
	/// The Remaining health for this player
	/// </summary>
	public  int remainingHealth;
	
	/// <summary>
	/// The Maximum Mana for this player
	/// </summary>
	public int totalMana ;
	
	/// <summary>
	/// The Remaining Mana for this player
	/// </summary>
	public int remainingMana ;
	
	/// <summary>
	/// The speed of the player, used for dodge probablility
	/// </summary>
	public int speed;
	
	/// <summary>
	/// The Armor for this player, used for damage reduction on hit
	/// </summary>
	public int armor;
	
	/// <summary>
	/// The base physical damage for this player (BEFORE ENCHANTMENTS) 
	/// </summary>
	public int physicalDamage ;
	
	/// <summary>
	/// The base magic damage for this player (BEFORE ENCHANTMENTS) 
	/// </summary>
	public int magicalDamage;

	/// <summary>
	/// The heal per turn.
	/// </summary>
	public int healPerTurn;

	/// <summary>
	/// The player's dollar balance usable at the store.
	/// </summary>
	public int dollarBalance;

	
	#endregion STATS
	#endregion Variables
	
	#region Public functions
	
	/// <summary>
	/// Used to pass a custom combination to this script from an outside source.
	/// </summary>
	/// <param name="incomingDamage">int Incoming damage applied to the player</param>
	public void TakeDamage(int incomingDamage){
		if ((incomingDamage - Mathf.FloorToInt(this.armor*0.5f)) > 0) {		// if damage is going to apply after armor
			if (this.remainingHealth - (incomingDamage - Mathf.FloorToInt(this.armor*0.5f)) > 0)
				this.remainingHealth -= (incomingDamage - Mathf.FloorToInt(this.armor*0.5f));
			else
				this.remainingHealth = 0;
		}
	}
	/// <summary>
	/// Heals the player for specified amount.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void HealPlayer(int amount){
		if (this.remainingHealth + amount > this.totalHealth) {
			this.remainingHealth = this.totalHealth;
		} else {
			this.remainingHealth += amount;
		}
		Debug.Log (this.name + " healed for " + amount + " " + this.remainingHealth);
	}
	/// <summary>
	/// Gives the specified amount of mana.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void GiveMana(int amount){
		if (this.remainingMana + amount >= this.totalMana) {
			this.remainingMana = this.totalMana;
		} else {
			this.remainingMana += amount;
		}
	}

	/// <summary>
	/// Uses the item at specified index.
	/// </summary>
	/// <param name="index">Index.</param>
	public void UseItem(int index){
		if (this.inventory == null) {
			this.inventory = GameObject.FindObjectOfType<InventoryGUIController>();
			this.inventory._items [index].ApplyEffect (this);
			// only use once if potion
			if (this.inventory._items [index].GetItemClass() == "Potion")
				this.inventory._items [index].used = true;
		}else{
			this.inventory._items [index].ApplyEffect (this);
			// only use once if potion
			if (this.inventory._items [index].GetItemClass() == "Potion")
				this.inventory._items [index].used = true;
		}
	}

	/// <summary>
	/// Gets the back hand transform.
	/// </summary>
	/// <returns>The back hand transform.</returns>
	public Transform GetBackHandTransform(){
		//body.spriteSheetElements[5] = shield
		// get all spriterenderers
		SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
		// get sword name
		string n = this.body.character.sprites [5].name;
		// find spriteRenderer
		Transform t=null;
		for (int i=0; i<s.Length; i++) {
			if (s[i].sprite.name == n){
				t = s[i].transform;
				return t;
			}
		}
		return t;
	}
	
	/// <summary>
	/// Gets the front hand transform.
	/// </summary>
	/// <returns>The front hand transform.</returns>
	public Transform GetFrontHandTransform(){
		//body.spriteSheetElements[7] = sword
		// get all spriterenderers
		SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
		
		// get sword name
		string n = this.body.character.sprites [7].name;
		// find spriteRenderer
		Transform t=null;
		for (int i=0; i<s.Length; i++) {
			if (s[i].sprite.name == n){
				t = s[i].transform;
				return t;
			}
		}
		return t;
	}
	
	/// <summary>
	/// Gets the body transform.
	/// </summary>
	/// <returns>The body transform.</returns>
	public Transform GetBodyTransform(){
		//body.spriteSheetElements[0] = body
		// get all spriterenderers
		SpriteRenderer[] s = this.body.character.GetComponentsInChildren<SpriteRenderer>();
		// get body name
		string n = this.body.character.sprites [0].name;
		// find spriteRenderer
		Transform t=null;
		for (int i=0; i<s.Length; i++) {
			if (s[i].sprite.name == n){
				t = s[i].transform;
				return t;
			}
		}
		return t;
	}
	
	/// <summary>
	/// Calls the SetWeapon method to set the weapon at the player's hand.
	/// </summary>
	/// <param name="name">Determines the weapon created. Sword, Axe, Bow, Hammer, Dagger, Spear.</param>
	public void CallSetWeapon(string name){
		SetWeapon(name);
	}
	
	/// <summary>
	/// Calls the SetArmor coroutine to set the armor at the player's body.
	/// </summary>
	/// <param name="name">Determines the armor created. Armor.</param>
	public void CallSetArmor(string name){
		SetArmor (name);
	}
	
	// deal damage to a player (makeshift game call)
	public void PhysicalAttack(PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		// animate sprites
		playerAnimator.SetTrigger(playerWeapon.GetAnimParameter());
		// apply damage to player
		attackedPlayer.TakeDamage (this.physicalDamage);
	}
	
	public bool MagicAttack(PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		// animate sprites
		// apply damage to player
		if (this.remainingMana - 10 >= 0) {
			this.remainingMana -= 10;
			attackedPlayer.TakeDamage (this.magicalDamage);
			return true;
		} else {
			Debug.Log ("Not Enough Mana For That!");
			return false;
		}
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

	#endregion Public functions
	
	#region Private functions
	
	private void SetWeapon(string name){
		if(playerHand.GetComponentInChildren<Weapon>() == true){				// If there is a weapon in hand..
			Destroy(playerHand.GetComponentInChildren<Weapon>().gameObject);	// .. Destroy it.
		}
		this.playerWeapon = ItemFactory.instance.CreateWeapon(playerHand, name);// Spawn specified weapon.
		this.playerWeapon.transform.parent = playerHand;										// Make it the child of the hand.
		this.playerWeapon.transform.localScale = new Vector3(1,1,1);							// Fix the scale.
		this.weaponComboScript = playerWeapon.GetComponentInChildren<CreateWeaponCombination>();// Sets a reference to the weapon's script.
	}
	
	private void SetArmor(string name){
		GameObject body = GetBodyTransform().gameObject;	// Gets a reference for the body to see if there..
		//Debug.Log (body);												// .. is a an armor on the character's body.
		if(body.GetComponentInChildren<Armor>() == true){				// If there is Armor on the body..
			Destroy(body.GetComponentInChildren<Armor>().gameObject);	// .. Destroy it.
		}
		this.playerArmor = ItemFactory.instance.CreateArmor(GetBodyTransform(), name);	// Spawn specified weapon.
		this.playerArmor.transform.parent = GetBodyTransform();								// Make it the child of the hand.
		this.playerArmor.transform.localScale = new Vector3(1,1,1);							// Fix the scale.
		if(this.playerArmor.transform.parent.GetComponent<SpriteRenderer>().enabled == true){	// Hide the default body if not hidden.
			this.playerArmor.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	#endregion Private functions
	
	#region MonoBehaviour 
	/// <summary>
	///Initialize the player with values	
	/// </summary>
	void Start(){
		// initialize player's Sprite
		
		this.body = GameObject.FindWithTag (this.tag).GetComponentInChildren<CreateCombination> ();
		this.playerAnimator = GetComponentInChildren<Animator>();
		this.dollarBalance = 50;
		// initialize weapon if player is supposed to have one
		CallSetWeapon("Sword");			// 
		//otherwise clear
		if (!spawnWithWeapon) {			//
			this.weaponComboScript.AllOff ();	// Creates a weapon, sets it to the player's hand and makes it invisible.
			this.weaponComboScript.SwapNow ();//
			//this.playerWeapon = null;
		}
	}
	#endregion MonoBehaviour
}

