using UnityEngine;
using System.Collections;

/// <summary>
/// Dagger class.
/// </summary>
public class Dagger : Weapon {
	/// <summary>
	/// The dagger type options.
	/// </summary>
	public string[] daggerOptionsTypes;
	/// <summary>
	/// The dagger type options sprites.
	/// </summary>
	public Sprite[] daggerOptionsSprites;
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
		GetDaggerType();
		SetItem(className, NameRandomizer.instance.GetPart1() + daggerOptionsTypes[typeIndex] + NameRandomizer.instance.GetPart2(),
		        daggerOptionsSprites[typeIndex], animationParameter, daggerOptionsTypes[typeIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}
	
	/// <summary>
	/// Gets the type of the sword.
	/// </summary>
	private void GetDaggerType(){
		typeIndex = Random.Range(0, daggerOptionsTypes.Length);
	}
}
