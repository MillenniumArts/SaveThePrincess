using UnityEngine;
using System.Collections;

/// <summary>
/// Bow class.
/// </summary>
public class Bow : Weapon{
	/// <summary>
	/// The bow type options.
	/// </summary>
	public string[] bowOptionsTypes;
	/// <summary>
	/// The bow type options' sprites.
	/// </summary>
	public Sprite[] bowOptionsSprites;
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

	/// <summary>
	/// Sets up this instance of the class.
	/// </summary>
	void Start(){
		factory = FindObjectOfType<ItemFactory>();
		GetBowType();
		SetItem(className, "Bow Name", bowOptionsSprites[typeIndex], animationParameter, bowOptionsTypes[typeIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}

	/// <summary>
	/// Gets the type of the bow.
	/// </summary>
	private void GetBowType(){
		typeIndex = Random.Range(0, bowOptionsTypes.Length);
	}
}
