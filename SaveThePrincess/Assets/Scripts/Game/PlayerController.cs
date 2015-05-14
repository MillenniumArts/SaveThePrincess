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

		Debug.Log (n);
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
		
		Debug.Log (n);
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



	#endregion Public functions

	#region Private functions
	#endregion Private functions

	#region MonoBehaviour 
	/// <summary>
	///Initialize the player with values	
	/// </summary>
	void Start(){
		// initialize player's Sprite

		this.body = GameObject.FindWithTag (this.tag).GetComponentInChildren<CreateCombination> ();
		this.body.random = true;

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

