﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Spear class.
/// </summary>
public class Spear : Weapon {
	/// <summary>
	/// The spear type options.
	/// </summary>
	public string[] spearOptionsTypes;
	/// <summary>
	/// The spear type options sprites.
	/// </summary>
	public Sprite[] spearOptionsSprites;
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
		GetSpearType();
        SetItem(className, NameRandomizer.instance.GetPart1() + this.GetWeaponType() + NameRandomizer.instance.GetPart2(),
		        spearOptionsSprites[typeIndex], animationParameter, idleAnimParameter, "Spear", spearOptionsTypes[typeIndex],
		        factory.GetStatusEffect(), 0, factory.GetModPwr(atkMin, atkMax), factory.GetModPwr(defMin,defMax),
		        factory.GetModPwr(spdMin, spdMax), factory.GetModPwr(hpMin, hpMax), factory.GetModPwr(manaMin, manaMax));
	}
	
	/// <summary>
	/// Gets the type of the spear.
	/// </summary>
	private void GetSpearType(){
		typeIndex = Random.Range(0, spearOptionsTypes.Length);
	}
}
