using UnityEngine;
using System.Collections;


public class PlayerController: MonoBehaviour {
	
	#region Variables
	/// <summary>
	/// Reference to the CreateCombination that is attached to this player, holds reference to Sprite
	/// </summary>
	public CreateCombination body;
	
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
	//public Item[] armorItems;
	
	///<summary>
	///Player Weapons - An array of Item objects that represents all weapon objects for the player
	///</summary>
	//public Item[] weaponItems;
	
	///<summary>
	///Player Usables - An array of Item objects that represents all usable objects for the player
	///</summary>
	//public Item[] usableItems;
	
	/// <summary>
	/// The inventory of the player.
	/// </summary>
	public InventoryController inventory;
	
	#endregion Inventory
	
	#region Enchantments
	///<summary>
	///Player Armor - An array of Item objects that represents all armor objects for the player
	///</summary>
	//public Item[] armorEnchants;
	
	///<summary>
	///Player Weapons - An array of Item objects that represents all weapon objects for the player
	///</summary>
	//public Item[] weaponEnchants;
	
	///<summary>
	///Player Usables - An array of Item objects that represents all usable objects for the player
	///</summary>
	//public Item[] usableEnchants;
	
	
	#endregion Enchantments
	
	
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
		if (this.remainingHealth - incomingDamage > 0)
			this.remainingHealth -= incomingDamage;
		else
			this.remainingHealth = 0;
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
		
		//Debug.Log (n);
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
		//string n = this.body.character.GetComponentsInChildren<SpriteRenderer>()[7].name;
		string n = this.body.character.sprites [7].name;
		//Debug.Log (n);
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
	/// Calls the SetWeapon coroutine to set the weapon at the player's hand.
	/// </summary>
	/// <param name="name">Determines the weapon created. Sword, Axe, Bow, Hammer, Dagger, Spear.</param>
	public void CallSetWeapon(string name){
		StartCoroutine("SetWeapon", name);
	}
	
	/// <summary>
	/// Calls the SetArmor coroutine to set the armor at the player's body.
	/// </summary>
	/// <param name="name">Determines the armor created. Armor.</param>
	public void CallSetArmor(string name){
		StartCoroutine("SetArmor", name);
	}
	
	// deal damage to a player (makeshift game call)
	public void PhysicalAttack(PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		// animate sprites
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

	#endregion Public functions
	
	#region Private functions
	
	/// <summary>
	/// Sets the weapon.
	/// </summary>
	/// <returns>WaitForSeconds(0.05f, then 0.01f)</returns>
	/// <param name="num">Weapon type.</param>
	private IEnumerator SetWeapon(string name){
		yield return new WaitForSeconds(0.05f);
		GameObject hand = GetFrontHandTransform().gameObject;			// Gets a reference for the hand to see if there..
		//Debug.Log (hand);												// .. is a weapon in the character's hand.
		if(hand.GetComponentInChildren<Weapon>() == true){				// If there is a weapon in hand..
			Destroy(hand.GetComponentInChildren<Weapon>().gameObject);	// .. Destroy it.
		}
		Weapon w = ItemFactory.instance.CreateWeapon(GetFrontHandTransform(), name);// Spawn specified weapon.
		w.transform.parent = GetFrontHandTransform();								// Make it the child of the hand.
		w.transform.localScale = new Vector3(1,1,1);								// Fix the scale.
		yield return new WaitForSeconds(0.01f);
		w.SwapTo(inventory._items[0]);												// Makes the new weapon to the inventory weapon.
		if(w.transform.parent.GetComponent<SpriteRenderer>().enabled == true){	// Hide the default sword if not hidden.
			w.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	private IEnumerator SetArmor(string name){
		yield return new WaitForSeconds(0.05f);
		GameObject body = GetBodyTransform().gameObject;	// Gets a reference for the body to see if there..
		//Debug.Log (body);												// .. is a an armor on the character's body.
		if(body.GetComponentInChildren<Armor>() == true){				// If there is Armor on the body..
			Destroy(body.GetComponentInChildren<Armor>().gameObject);	// .. Destroy it.
		}
		Armor a = ItemFactory.instance.CreateArmor(GetBodyTransform(), name);	// Spawn specified weapon.
		a.transform.parent = GetBodyTransform();								// Make it the child of the hand.
		a.transform.localScale = new Vector3(1,1,1);							// Fix the scale.
		yield return new WaitForSeconds(0.01f);
		a.SwapTo(inventory._items[1]);											// Makes the new armor to the inventory armor.
		if(a.transform.parent.GetComponent<SpriteRenderer>().enabled == true){	// Hide the default body if not hidden.
			a.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
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
		//this.body.random = true;
		this.dollarBalance = 50;
		
		//Item item = ItemFactory.instance.CreateWeapon(playerHand);
		
		// Stat setup
		// THIS SHOULD BE DIFFERENT DEPENDING ON TYPE OF PLAYER!
		
		// Item Setup
		
		// this.armorItems = new Item[numArmorItems];
		// this.weaponItems = new Item[numWeaponItems];
		// this.usableItems = new Item[numUsableItems];
		
		// Enchant Setup
		// this.armorEnchants = new Item[numArmorItems];
		// this.weaponEnchants = new Item[numWeaponItems];
		// this.usableEnchants = new Item[numUsableItems];
	}
	
	
	#endregion MonoBehaviour
}

