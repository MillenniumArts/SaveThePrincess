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
	/// The animation parameter for item use.
	/// </summary>
	public string animParameter;
	/// <summary>
    /// The idle animation parameter (TwoHandedIdle versus OneHandedIdle).
	/// </summary>
	public string idleAnimParameter;
	/// <summary>
	/// The state of the idle animation.
	/// </summary>
	public bool idleState;
	/// <summary>
	/// The sub class of the item. From the item class of Weapon we have Sword, Axe, Bow, etc.
	/// </summary>
	public string itemSubClass;
	/// <summary>
	/// The sub sub class of the item.  From the Sword class we have Katana, Broadsword, Lightsaber, etc.
	/// </summary>
	public string itemSubSubClass;
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
	/// <summary>
	/// The dollar cost of the item in the store.
	/// </summary>
	public int dollarCost;
	/// <summary>
	/// whether or not the item has been used.
	/// </summary>
	public bool used;
	#endregion Variables
	
	#region Item manipulation
	/// <summary>
	/// Sets the item.  Somewhat replaces a constructor.
	/// </summary>
	/// <param name="c">Item class. "Weapon", "Armor", "Magic", "Potion".</param>
	/// <param name="n">Item name.</param>
	/// <param name="s">The Sprite.</param>
	/// <param name="a">The animation parameter.</param>
	/// <param name="ia">The idle animation parameter.</param>
	/// <param name="sC">The sub-class of item with an item class. Within Weapon, "Sword", "Axe", "Bow".</param>
	/// <param name="ssC">The sub-sub-class of item with an item class. Within Sword, "Katana", "Broadsword", "Lightsaber".</param>
	/// <param name="st">Status effect of the item.</param>
	/// <param name="h">The healing power if it is a potion or healing spell.</param>
	/// <param name="atk">Atk modifier.  Adds to the player's base stats.</param>
	/// <param name="def">Def modifier.  Adds to the player's base stats.</param>
	/// <param name="spd">Spd modifier.  Adds to the player's base stats.</param>
	/// <param name="hp">Hp modifier.  Adds to the player's base stats.</param>
	/// <param name="mana">Mana modifier.  Adds to the player's base stats.</param>
	public void SetItem(string c, string n, Sprite s, string a, string ia, string sC, string ssC, string st, int h, int atk, int def, int spd, int hp, int mana){
		itemClass = c;
		itemName = n;
		image = s;
		idleAnimParameter = ia;
		animParameter = a;
		itemSubClass = sC;
		itemSubSubClass = ssC;
		statusEffect = st;
		healEffect = h;
		atkMod = atk;
		defMod = def;
		spdMod = spd;
		hpMod = hp;
		manaMod = mana;
		//dollarCost = 0;
	}
	
	/// <summary>
	/// Clears the stats.
	/// </summary>
	public void ClearStats(){
		itemClass = "";
		itemName = "";
		image = null;
		animParameter = "";
		itemSubClass = "";
		itemSubSubClass = "";
		statusEffect = "";
		healEffect = 0;
		atkMod = 0;
		defMod = 0;
		spdMod = 0;
		hpMod = 0;
		manaMod = 0;
		dollarCost = 0;
	}
	
	/// <summary>
	/// Behaves like an overloaded operator=. Copies all data from one item to another.
	/// </summary>
	/// <param name="i2">Item to copy data from</param>
	public void SwapTo(Item i2){
		itemClass = i2.GetItemClass();
		itemName = i2.GetName();
		image = i2.GetSprite();
		animParameter = i2.GetAnimParameter();
		itemSubClass = i2.GetItemSubClass();
		itemSubSubClass = i2.GetSubSubClass();
		statusEffect = i2.GetStatusEffect();
		healEffect = i2.GetHealEffect();
		atkMod = i2.GetAtkMod();
		defMod = i2.GetDefMod();
		spdMod = i2.GetSpdMod();
		hpMod = i2.GetHpMod();
		manaMod = i2.GetManaMod();
		dollarCost = i2.GetDollarCost ();
		// If the intance of the item has a Sprite Renderer, swap the sprite.
		if(this.GetComponent<SpriteRenderer>() == true){
			this.GetComponent<SpriteRenderer>().sprite = image;
		}
	}
	/// <summary>
	/// Sets the price, called by ItemFactory on creation.
	/// </summary>
	public void setCost(int cost){
		this.dollarCost = cost;
	}

	#endregion Item manipulation

	#region Effects
	/// <summary>
	/// Determines whether this instance can apply effects on the specified player.
	/// </summary>
	/// <returns><c>true</c> if this instance can apply effects the specified p; otherwise, <c>false</c>.</returns>
	/// <param name="p">P.</param>
	public bool CanApplyEffectsToSelf(PawnController p){
		if (this.GetItemClass () == "Potion") {
			if (this.GetItemSubClass() == "HealPotion"){
				if (p.remainingHealth < p.totalHealth){
					return true;
				}
			}else if (this.GetItemSubClass() == "ManaPotion"){
				if (p.remainingEnergy < p.totalEnergy){
					return true;
				}
			}
		} else if (this.GetItemClass () == "Magic") {
			if (this.GetItemSubClass() == "HealMagic"){
				if (p.remainingHealth < p.totalHealth){
					return true;
				}
			}else if (this.GetItemSubClass() == "AttackMagic"){
				return false;
			}
		}
		return false;
	}

	/// <summary>
	/// Applies the effect.
	/// </summary>
	/// <returns><c>true</c>, if effect was applied, <c>false</c> otherwise.</returns>
	/// <param name="p">P.</param>
    public bool ApplyEffect(PawnController p)
    {
        if (this.GetItemClass() == "Potion")
        {
            if (this.GetItemSubClass() == "HealPotion")
            {
                if (p.remainingHealth < p.totalHealth)
                {
                    p.HealForAmount(this.GetHealEffect());
                    this.used = true;
                    return true;
                }
                else
                    return false;
            }
            else if (this.GetItemSubClass() == "ManaPotion")
            {
                if (p.remainingEnergy < p.totalEnergy)
                {
                    p.GiveEnergy(this.GetManaMod());
                    this.used = true;
                    return true;
                }
                else
                    return false;
            }
            // end potions
        }
        else if (this.GetItemClass() == "Magic")
        {
            if (p.CanUseEnergy(10))
            {
                if (this.GetItemSubClass() == "HealMagic")
                {
                    p.HealForPercent((this.GetHealEffect() / 100f));
                }
                else if (this.GetItemSubClass() == "AttackMagic")
                {
                    // apply damage to player
                    p.SetDamage(this.GetAtkMod(), false);
                }
                return true;
            }
            else
            {
                return false;
            }
		} else {
			Debug.Log ("Unable to Apply Effect of " + this.itemName);
			return false;
		}
		return false;
	}
	#endregion Effects

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
	public string GetItemSubClass(){
		return itemSubClass;
	}
	public string GetSubSubClass(){
		return itemSubSubClass;
	}
	public string GetStatusEffect(){
		return statusEffect;
	}
	public int GetHealEffect(){
		return healEffect;
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
	public int GetDollarCost(){
		return dollarCost;
	}
	public string GetStatsString(){
		string statsString = GetName () + "\n" +
			//"Type: " + GetItemSubClass() + "\n" +
				"DMG: +" + GetAtkMod() + " | " + 
				"AMR: +" + GetDefMod() + "\n" +
				"Cost: $" + GetDollarCost() + "\n";
		return statsString;
	}
	#endregion Getters
	
}
