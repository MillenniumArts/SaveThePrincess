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

	#region Items
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
	#endregion Items

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
	public int physicalDamage;

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

	#endregion Public functions

	#region Private functions
	#endregion Private functions

	#region MonoBehaviour 
	/// <summary>
	///Initialize the player with values	
	/// </summary>
	void Start(){
		// initialize player's Sprite
		//* SHOULD THIS BE IN EDITOR?
		this.body = this.gameObject.GetComponent<CreateCombination> ();

		// Stat setup
		// THIS SHOULD BE DIFFERENT DEPENDING ON TYPE OF PLAYER!
		this.totalHealth = 100;
		this.remainingHealth = totalHealth;
				
		this.armor = 10;
		this.physicalDamage = 15;
		this.magicalDamage = 15;
		this.totalMana = 25;
		this.remainingMana = totalMana;
		this.speed = 1;
		
		this.numArmorItems = 2;
		this.numUsableItems = 2;
		this.numWeaponItems = 2;
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

