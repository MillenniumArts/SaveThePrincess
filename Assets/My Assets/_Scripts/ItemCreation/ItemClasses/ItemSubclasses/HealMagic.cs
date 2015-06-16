using UnityEngine;
using System.Collections;

/// <summary>
/// Heal magic class.
/// </summary>
public class HealMagic : Magic {
	/// <summary>
	/// The heal magic type options.
	/// </summary>
	public string[] healMagicOptionsTypes;
	/// <summary>
	/// The heal magic type options' sprites.
	/// </summary>
	public Sprite[] healMagicOptionsSprites;
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
		GetHealMagicType();
		SetItem(className, "HealMagic Name", healMagicOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "HealMagic", healMagicOptionsTypes[typeIndex],
		        "none", factory.GetHealPwr(healMin, healMax), 0, 0, 0, 0, 0);
	}
	
	/// <summary>
	/// Gets the type of the heal magic.
	/// </summary>
	private void GetHealMagicType(){
		typeIndex = Random.Range(0, healMagicOptionsTypes.Length);
	}
}
