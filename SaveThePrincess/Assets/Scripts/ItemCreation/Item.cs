using UnityEngine;
using System.Collections;

/// <summary>
/// The Item class.  Holds all the information about items be they Weapons, Armor, Magic, or Potions.
/// </summary>
public class Item : MonoBehaviour {

	#region Variables
	/// <summary>
	/// Reference to the ItemFactory.
	/// </summary>
	protected ItemFactory factory;
	/* Right now the variables are public so that we can see them in the inspector. To be fixed. */
	/// <summary>
	/// The item class.  Weapon, Armor, Magic, Potion.  More item classes can be added later.
	/// Needs a better name so as to not be confused with C# classes.
	/// </summary>
	public string itemClass;
	/// <summary>
	/// The name of the item. TO be gotten from a random name script.
	/// </summary>
	public string itemName;
	/// <summary>
	/// The sprite for the item. TODO: Figure out how to connect the sprite, weapon type and animation parameter so they follow each other.
	/// </summary>
	public Sprite image;
	/// <summary>
	/// The animation parameter.
	/// </summary>
	public string animParameter;
	/// <summary>
	/// The type of the item. From the item class of Weapon we have Sword, Axe, Bow, etc.
	/// </summary>
	public string itemType;
	/// <summary>
	/// The status effect of the weapon. Poisoned sword, Fire sword, Ice sword, for example. 
	/// </summary>
	public string statusEffect;
	/// <summary>
	/// The healing effect. If it is a potion or healing spell how much will it heal.
	/// </summary>
	public int healEffect;
	/// <summary>
	/// The atk modifier.  Adds to the player's base stats.
	/// </summary>
	public int atkMod;
	/// <summary>
	/// The def modifier.  Adds to the player's base stats.
	/// </summary>
	public int defMod;
	/// <summary>
	/// The spd modifier.  Adds to the player's base stats.
	/// </summary>
	public int spdMod;
	/// <summary>
	/// The hp modidier.  Adds to the player's base stats.
	/// </summary>
	public int hpMod;
	/// <summary>
	/// The mana modifier.  Adds to the player's base stats.
	/// </summary>
	public int manaMod;
	#endregion Variables

	#region Item manipulation
	/// <summary>
	/// Sets the item.  Somewhat replaces a constructor.
	/// </summary>
	/// <param name="c">Item class. "Weapon", "Armor", "Magic", "Potion".</param>
	/// <param name="n">Item name.</param>
	/// <param name="s">The Sprite.</param>
	/// <param name="a">The animation parameter.</param>
	/// <param name="t">The type of item with an item class. Within Weapon, "Sword", "Axe", "Bow".</param>
	/// <param name="st">Status effect of the item.</param>
	/// <param name="h">The healing power if it is a potion or healing spell.</param>
	/// <param name="atk">Atk modifier.  Adds to the player's base stats.</param>
	/// <param name="def">Def modifier.  Adds to the player's base stats.</param>
	/// <param name="spd">Spd modifier.  Adds to the player's base stats.</param>
	/// <param name="hp">Hp modifier.  Adds to the player's base stats.</param>
	/// <param name="mana">Mana modifier.  Adds to the player's base stats.</param>
	public void SetItem(string c, string n, Sprite s, string a, string t, string st, int h, int atk, int def, int spd, int hp, int mana){
		itemClass = c;
		itemName = n;
		image = s;
		animParameter = a;
		itemType = t;
		statusEffect = st;
		healEffect = h;
		atkMod = atk;
		defMod = def;
		spdMod = spd;
		hpMod = hp;
		manaMod = mana;
	}

	/// <summary>
	/// Clears the stats.
	/// </summary>
	public void ClearStats(){
		itemClass = "";
		itemName = "";
		image = null;
		animParameter = "";
		itemType = "";
		statusEffect = "";
		healEffect = 0;
		atkMod = 0;
		defMod = 0;
		spdMod = 0;
		hpMod = 0;
		manaMod = 0;
	}
	
	/// <summary>
	/// Behaves like an overloaded operator=. Copies all data from one item to another.
	/// </summary>
	/// <param name="i2">Item to copy data from</param>
	public void SwapTo(Item i2){
		itemClass = i2.itemClass;
		itemName = i2.itemName;
		image = i2.image;
		animParameter = i2.animParameter;
		itemType = i2.itemType;
		statusEffect = i2.statusEffect;
		healEffect = i2.healEffect;
		atkMod = i2.atkMod;
		defMod = i2.defMod;
		spdMod = i2.spdMod;
		hpMod = i2.hpMod;
		manaMod = i2.manaMod;
	}
	#endregion Item manipulation

	#region Getters
	public string GetItemClass(){
		return itemClass;
	}
	public string GetName(){
		return itemName;
	}
	public Sprite GetSprite(){
		return image;
	}
	public string GetSpriteName(){
		return image.name;
	}
	public string GetAnimParameter(){
		return animParameter;
	}
	public string GetItemType(){
		return itemType;
	}
	public string GetStatusEffect(){
		return statusEffect;
	}
	public int GetAtkMod(){
		return atkMod;
	}
	public int GetDefMod(){
		return defMod;
	}
	public int GetSpdMod(){
		return spdMod;
	}
	public int GetHpMod(){
		return hpMod;
	}
	public int GetManaMod(){
		return manaMod;
	}
	#endregion Getters

}
