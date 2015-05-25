using UnityEngine;
using System.Collections;

/// <summary>
/// Axe class.
/// </summary>
public class Axe : Weapon {
	/// <summary>
	/// The axe type options.
	/// </summary>
	public string[] axeOptionsTypes;
	/// <summary>
	/// The axe type options' sprites.
	/// </summary>
	public Sprite[] axeOptionsSprites;
	/// <summary>
	/// The index of the type.
	/// </summary>
	private int subClassIndex;
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
		GetAxeType();
		SetItem(className,NameRandomizer.instance.GetPart1() + axeOptionsTypes[subClassIndex] + NameRandomizer.instance.GetPart2(),
		        axeOptionsSprites[subClassIndex], animationParameter, "Axe", axeOptionsTypes[subClassIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}

	/// <summary>
	/// Gets the type of the axe.
	/// </summary>
	private void GetAxeType(){
		subClassIndex = Random.Range(0, axeOptionsTypes.Length);
	}
}
