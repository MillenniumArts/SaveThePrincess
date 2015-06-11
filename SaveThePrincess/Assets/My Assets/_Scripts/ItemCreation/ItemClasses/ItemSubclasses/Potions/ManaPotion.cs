using UnityEngine;
using System.Collections;

/// <summary>
/// Heal potion class.
/// </summary>
public class ManaPotion : Potion {
	/// <summary>
	/// The heal potion type options.
	/// </summary>
	public string[] manaPotionOptionsTypes;
	/// <summary>
	/// The heal potion type options' sprites.
	/// </summary>
	public Sprite[] manaPotionOptionsSprites;
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
		SetItem(className, NameRandomizer.instance.GetPart1() +  manaPotionOptionsTypes[typeIndex] + NameRandomizer.instance.GetPart2(),
		        manaPotionOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "ManaPotion", manaPotionOptionsTypes[typeIndex],
		        "none", 0, 0, 0, 0, 0, factory.GetManaPwr(manaMin, manaMax));
	}

}
