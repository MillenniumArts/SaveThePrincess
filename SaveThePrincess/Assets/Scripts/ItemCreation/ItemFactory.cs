using UnityEngine;
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
	public Item CreateWeapon(Transform spawnPoint){
		int randomNum = Random.Range(0, 3);
		Item w = blankSwordPrefab;
		switch(randomNum){
		case 0: w = CreateSword(spawnPoint);
			break;
		case 1: w = CreateAxe(spawnPoint);
			break;
		case 2: w = CreateBow(spawnPoint);
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
	public Item CreateArmor(Transform spawnPoint){
		int randomNum = Random.Range(0, 3);
		Item a = blankBodyArmorPrefab;
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
	public Item CreatePotion(Transform spawnPoint){
		int randomNum = Random.Range(0, 3);
		Item p = blankHealPotionPrefab;
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
	private Item CreateSword(Transform spawnPoint){
		Item i = Instantiate(blankSwordPrefab, spawnPoint.position, Quaternion.identity) as Weapon;
		return i;
	}
	private Item CreateAxe(Transform spawnPoint){
		Item i = Instantiate(blankAxePrefab, spawnPoint.position, Quaternion.identity) as Weapon;
		return i;
	}
	private Item CreateBow(Transform spawnPoint){
		Item i = Instantiate(blankBowPrefab, spawnPoint.position, Quaternion.identity) as Weapon;
		return i;
	}

	// Armor Creators
	private Item CreateBodyArmor(Transform spawnPoint){
		Item i = Instantiate(blankBodyArmorPrefab, spawnPoint.position, Quaternion.identity) as Armor;
		return i;
	}

	// Magic Creators
	private Item CreateAttackMagic(Transform spawnPoint){
		Item i = Instantiate(blankAtkMagPrefab, spawnPoint.position, Quaternion.identity) as Magic;
		return i;
	}
	private Item CreateHealMagic(Transform spawnPoint){
		Item i = Instantiate(blankHealMagPrefab, spawnPoint.position, Quaternion.identity) as Magic;
		return i;
	}

	// Potion Creators
	private Item CreateHealPotion(Transform spawnPoint){
		Item i = Instantiate(blankHealPotionPrefab, spawnPoint.position, Quaternion.identity) as Potion;
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
