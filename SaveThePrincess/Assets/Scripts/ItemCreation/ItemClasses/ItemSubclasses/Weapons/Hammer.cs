using UnityEngine;
using System.Collections;

/// <summary>
/// Hammer class.
/// </summary>
public class Hammer : Weapon {
	/// <summary>
	/// The hammer type options.
	/// </summary>
	public string[] hammerOptionsTypes;
	/// <summary>
	/// The hammer type options sprites.
	/// </summary>
	public Sprite[] hammerOptionsSprites;
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
		GetHammerType();
		SetItem(className, NameRandomizer.instance.GetPart1() + hammerOptionsTypes[typeIndex] + NameRandomizer.instance.GetPart2(),
		        hammerOptionsSprites[typeIndex], animationParameter, "Hammer", hammerOptionsTypes[typeIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}
	
	/// <summary>
	/// Gets the type of the hammer.
	/// </summary>
	private void GetHammerType(){
		typeIndex = Random.Range(0, hammerOptionsTypes.Length);
	}
}
