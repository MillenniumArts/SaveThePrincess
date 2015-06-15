using UnityEngine;
using System.Collections;

/// <summary>
/// Heal potion class.
/// </summary>
public class HealPotion : Potion {
	/// <summary>
	/// The heal potion type options.
	/// </summary>
	public string[] healPotionOptionsTypes;
	/// <summary>
	/// The heal potion type options' sprites.
	/// </summary>
	public Sprite[] healPotionOptionsSprites;
	/// <summary>
	/// The index of the type.
	/// </summary>
	private int typeIndex;
	// Minimums and maximums of the stats.
	public int atkMin;
	public int atkMax;
	public int defMin;
	public int defMax;
	public int spdMin;
	public int spdMax;
	public int hpMin;
	public int hpMax;
	public int manaMin;
	public int manaMax;
	public int healMin;
	public int healMax;
	
	/// <summary>
	/// Sets up this instance of the class.
	/// </summary>
	void Start(){
		factory = FindObjectOfType<ItemFactory>();
		typeIndex = 0;
		SetItem(className, NameRandomizer.instance.GetPart1() +  healPotionOptionsTypes[typeIndex] + NameRandomizer.instance.GetPart2(),
		        healPotionOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "HealPotion", healPotionOptionsTypes[typeIndex],
		        "none", factory.GetHealPwr(healMin, healMax), 0, 0, 0, 0, 0);
	}
	
	/// <summary>
	/// Gets the type of the heal potion.
	/// </summary>
	private void GetHealPotionType(){
		typeIndex = Random.Range(0, healPotionOptionsTypes.Length);
	}
}
