using UnityEngine;
using System.Collections;

/// <summary>
/// Body armor class.
/// </summary>
public class BodyArmor : Armor {
	/// <summary>
	/// The body armor type options.
	/// </summary>
	public string[] bodyArmorOptionsTypes;
	/// <summary>
	/// The body armor type options' sprites.
	/// </summary>
	public Sprite[] bodyArmorOptionsSprites;
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
		GetBodyArmorType();
		SetItem(className, "BodyArmor Name", bodyArmorOptionsSprites[typeIndex], animationParameter, bodyArmorOptionsTypes[typeIndex],
		        "none", 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}

	/// <summary>
	/// Gets the type of the body armor.
	/// </summary>
	private void GetBodyArmorType(){
		typeIndex = Random.Range(0, bodyArmorOptionsTypes.Length);
	}
}
