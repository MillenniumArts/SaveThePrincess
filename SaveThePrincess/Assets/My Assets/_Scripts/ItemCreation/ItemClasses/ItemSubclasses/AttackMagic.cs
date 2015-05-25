using UnityEngine;
using System.Collections;

/// <summary>
/// Attack magic class.
/// </summary>
public class AttackMagic : Magic {
	/// <summary>
	/// The attack magic type options.
	/// </summary>
	public string[] attackMagicOptionsTypes;
	/// <summary>
	/// The attack magic type options' sprites.
	/// </summary>
	public Sprite[] attackMagicOptionsSprites;
	/// <summary>
	/// The index of the type.
	/// </summary>
	private int typeIndex;
	// Minimums and maximums for the stats.
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
		GetAttackMagicType();
		SetItem(className, "AttackMagic Name", attackMagicOptionsSprites[typeIndex], animationParameter, "AttackMagic", attackMagicOptionsTypes[typeIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}
	
	/// <summary>
	/// Gets the type of the attack magic.
	/// </summary>
	private void GetAttackMagicType(){
		typeIndex = Random.Range(0, attackMagicOptionsTypes.Length);
	}
}
