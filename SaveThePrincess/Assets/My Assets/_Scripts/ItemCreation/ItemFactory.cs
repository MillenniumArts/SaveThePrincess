﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Item factory.  Creates instances of items.
/// </summary>
public class ItemFactory : MonoBehaviour{

	#region Singleton
	private static ItemFactory _instance;

	public static ItemFactory instance{
		get{
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<ItemFactory>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	void Awake(){
		if(_instance == null){
			_instance = this;
			DontDestroyOnLoad(this);
		}else{
			if(this != _instance){
				Destroy(this.gameObject);
			}
		}
	}
	#endregion Singleton

	#region Variables
	/// <summary>
	/// The status types.  Can be added to and changed in the Inspector.
	/// </summary>
	[SerializeField] private string[] statusTypes = {"None", "Burned", "Frozen", "Paralysed", "Confused", "Poisoned"};
	/*
	 * References to prefabs of blank template items.
	 */
	public Sword blankSwordPrefab;
	public Axe blankAxePrefab;
	public Bow blankBowPrefab;
	public Hammer blankHammerPrefab;
	public Dagger blankDaggerPrefab;
	public Spear blankSpearPrefab;
	public BodyArmor blankBodyArmorPrefab;
	public AttackMagic blankAtkMagPrefab;
	public HealMagic blankHealMagPrefab;
	public HealPotion blankHealPotionPrefab;
	#endregion Variables

	#region Item Creation
	/* NOTE TODO: Change the hard coded random ranges */

	/// <summary>
	/// Creates a random weapon from available weapon types.
	/// </summary>
	/// <returns>The weapon.</returns>
	/// <param name="spawnPoint">Spawn point.</param>
	/// <summary>
	/// Creates a random weapon from available weapon types.
	/// </summary>
	/// <returns>The weapon.</returns>
	/// <param name="spawnPoint">Spawn point.</param>
	/// <param name="num">Number dictates the type of weapon created.  0 Sword, 1 Axe, 2 Bow, 3 Hammer, 4 Dagger, 5 Spear.</para>
	public Weapon CreateWeapon(Transform spawnPoint, string name){
		//int randomNum = Random.Range(0, 6);
		Weapon w = blankSwordPrefab;
		switch(name){
		case "Sword": w = CreateSword(spawnPoint);
			break;
		case "Axe": w = CreateAxe(spawnPoint);
			break;
		case "Bow": w = CreateBow(spawnPoint);
			break;
		case "Hammer": w = CreateHammer(spawnPoint);
			break;
		case "Dagger": w = CreateDagger(spawnPoint);
			break;
		case "Spear": w = CreateSpear(spawnPoint);
			break;
		default:
			Debug.Log ("No weapons to create!");
			break;
		}
		return w;
	}
	
	/// <summary>
	/// Creates a random armor from available armor types.
	/// </summary>
	/// <returns>The armor.</returns>
	/// <param name="spawnPoint">Spawn point.</param>
	public Armor CreateArmor(Transform spawnPoint, string name){
		//int randomNum = Random.Range(0, 3);
		Armor a = blankBodyArmorPrefab;
		switch(name){
		case "Armor": a = CreateBodyArmor(spawnPoint);
			break;
		case "other": a = CreateBodyArmor(spawnPoint);
			break;
		case "other2": a = CreateBodyArmor(spawnPoint);
			break;
		default:
			Debug.Log ("No armor to create!");
			break;
		}
		return a;
	}

	/// <summary>
	/// Creates a random magic spell from available magic types.
	/// </summary>
	/// <returns>The magic.</returns>
	/// <param name="spawnPoint">Spawn point.</param>
	public Item CreateMagic(Transform spawnPoint, string name){
		//int randomNum = Random.Range(0, 3);
		Item m = blankAtkMagPrefab;
		switch(name){
		case "AttackMagic": m = CreateAttackMagic(spawnPoint);
			break;
		case "HealMagic": m = CreateHealMagic(spawnPoint);
			break;
		case "ManaHealMagic": m = CreateHealMagic(spawnPoint);
			break;
		default:
			Debug.Log ("No magic to create!");
			break;
		}
		return m;
	}

	/// <summary>
	/// Creates a random potion from available potion types.
	/// </summary>
	/// <returns>The potion.</returns>
	/// <param name="spawnPoint">Spawn point.</param>
	public Potion CreatePotion(Transform spawnPoint, string name){
		//int randomNum = Random.Range(0, 3);
		Potion p = blankHealPotionPrefab;
		switch(name){
		case "HealPotion": p = CreateHealPotion(spawnPoint);
			break;
		case "other": p = CreateHealPotion(spawnPoint);
			break;
		case "other2": p = CreateHealPotion(spawnPoint);
			break;
		default:
			Debug.Log ("No potion to create!");
			break;
		}
		return p;
	}

	// Weapon Creators
	private Weapon CreateSword(Transform spawnPoint){
		Weapon i = Instantiate(blankSwordPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Weapon;
		return i;
	}
	private Weapon CreateAxe(Transform spawnPoint){
		Weapon i = Instantiate(blankAxePrefab, spawnPoint.position, spawnPoint.transform.rotation) as Weapon;
		return i;
	}
	private Weapon CreateBow(Transform spawnPoint){
		Weapon i = Instantiate(blankBowPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Weapon;
		return i;
	}
	private Weapon CreateHammer(Transform spawnPoint){
		Weapon i = Instantiate(blankHammerPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Weapon;
		return i;
	}
	private Weapon CreateDagger(Transform spawnPoint){
		Weapon i = Instantiate(blankDaggerPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Weapon;
		return i;
	}
	private Weapon CreateSpear(Transform spawnPoint){
		Weapon i = Instantiate(blankSpearPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Weapon;
		return i;
	}

	// Armor Creators
	private Armor CreateBodyArmor(Transform spawnPoint){
		Armor i = Instantiate(blankBodyArmorPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Armor;
		return i;
	}

	// Magic Creators
	private Magic CreateAttackMagic(Transform spawnPoint){
		Magic i = Instantiate(blankAtkMagPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Magic;
		return i;
	}
	private Magic CreateHealMagic(Transform spawnPoint){
		Magic i = Instantiate(blankHealMagPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Magic;
		return i;
	}

	// Potion Creators
	private Potion CreateHealPotion(Transform spawnPoint){
		Potion i = Instantiate(blankHealPotionPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Potion;
		return i;
	}
	#endregion  Item Creation
	

	#region Randomizers
	/// <summary>
	/// Gets the an int value that will modify a stat.
	/// </summary>
	/// <returns>The modifier value.</returns>
	/// <param name="m">Minimum value.</param>
	/// <param name="M">Maximum value.</param>
	public int GetModPwr(int m, int M){
		// Minimum and maximum is multiplied by the dificulty level.
		int mod = Random.Range(m * DifficultyLevel.GetInstance().DifficultyMultiplier(), M * DifficultyLevel.GetInstance().DifficultyMultiplier());
		return mod;
	}
	/// <summary>
	/// Gets the healing power modifier.
	/// </summary>
	/// <returns>The heal power modifier.</returns>
	/// <param name="m">Minimum value.</param>
	/// <param name="M">Maximum value.</param>
	public int GetHealPwr(int m, int M){
		int h = Random.Range (m,M);
		return h;
	}
	/// <summary>
	/// Gets a random status effect.
	/// </summary>
	/// <returns>The status effect.</returns>
	public string GetStatusEffect(){
		string e = statusTypes[Random.Range(0, statusTypes.Length)];
		return e;
	}
	/// <summary>
	/// Gets the random dollar value to attach to an item.
	/// </summary>
	/// <returns>The random dollar value.</returns>
	/// <param name="min">Minimum value.</param>
	/// <param name="max">Max value.</param>
	public int getRandomDollarValue(int min, int max){
		return Random.Range (min, max);
	}

	#endregion Randomizers
}
