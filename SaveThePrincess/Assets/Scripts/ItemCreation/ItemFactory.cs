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
	public Weapon CreateWeapon(Transform spawnPoint){
		int randomNum = Random.Range(0, 6);
		Weapon w = blankSwordPrefab;
		switch(randomNum){
		case 0: w = CreateSword(spawnPoint);
			break;
		case 1: w = CreateAxe(spawnPoint);
			break;
		case 2: w = CreateBow(spawnPoint);
			break;
		case 3: w = CreateHammer(spawnPoint);
			break;
		case 4: w = CreateDagger(spawnPoint);
			break;
		case 5: w = CreateSpear(spawnPoint);
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
	public Armor CreateArmor(Transform spawnPoint){
		int randomNum = Random.Range(0, 3);
		Armor a = blankBodyArmorPrefab;
		switch(randomNum){
		case 0: a = CreateBodyArmor(spawnPoint);
			break;
		case 1: a = CreateBodyArmor(spawnPoint);
			break;
		case 2: a = CreateBodyArmor(spawnPoint);
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
	public Item CreateMagic(Transform spawnPoint){
		int randomNum = Random.Range(0, 3);
		Item m = blankAtkMagPrefab;
		switch(randomNum){
		case 0: m = CreateAttackMagic(spawnPoint);
			break;
		case 1: m = CreateHealMagic(spawnPoint);
			break;
		case 2: m = CreateAttackMagic(spawnPoint);
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
	public Potion CreatePotion(Transform spawnPoint){
		int randomNum = Random.Range(0, 3);
		Potion p = blankHealPotionPrefab;
		switch(randomNum){
		case 0: p = CreateHealPotion(spawnPoint);
			break;
		case 1: p = CreateHealPotion(spawnPoint);
			break;
		case 2: p = CreateHealPotion(spawnPoint);
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
	private Item CreateAttackMagic(Transform spawnPoint){
		Item i = Instantiate(blankAtkMagPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Magic;
		return i;
	}
	private Item CreateHealMagic(Transform spawnPoint){
		Item i = Instantiate(blankHealMagPrefab, spawnPoint.position, spawnPoint.transform.rotation) as Magic;
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
	#endregion Randomizers
}