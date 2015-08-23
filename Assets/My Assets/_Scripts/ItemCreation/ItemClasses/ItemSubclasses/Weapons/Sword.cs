using UnityEngine;
using System.Collections;

/// <summary>
/// Sword class.
/// </summary>
public class Sword : Weapon {
	/// <summary>
	/// The sword type options.
	/// </summary>
	public string[] swordOptionsTypes;
	/// <summary>
	/// The sword type options sprites.
	/// </summary>
	public Sprite[] swordOptionsSprites;
	/// <summary>
	/// The index of the type.
	/// </summary>
	private int typeIndex;
	// The minimums and maximums of stats.
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

	/// <summary>
	/// Sets up this instance of the class.
	/// </summary>
	void Start(){
		factory = FindObjectOfType<ItemFactory>();
		GetSwordType();
        SetItem(className, NameRandomizer.instance.GetPart1() + this.GetWeaponType() + NameRandomizer.instance.GetPart2(),
		        swordOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "Sword", swordOptionsTypes[typeIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}

	/// <summary>
	/// Gets the type of the sword.
	/// </summary>
	private void GetSwordType(){
		typeIndex = Random.Range(0, swordOptionsTypes.Length);
	}
}
