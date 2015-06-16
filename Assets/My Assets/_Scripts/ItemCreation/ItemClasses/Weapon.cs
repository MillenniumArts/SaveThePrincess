using UnityEngine;
using System.Collections;

/// <summary>
/// Weapon class.
/// </summary>
public class Weapon : Item {
	/// <summary>
	/// The name of the class. Weapon.
	/// </summary>
	public string className = "Weapon";
	/// <summary>
	/// The animation parameter for the Weapon class.
	/// </summary>
	public string animationParameter;
	/// <summary>
	/// The weapon combination.
	/// </summary>
	public string[] weaponCombination;

	/// <summary>
	/// Sets the combination.
	/// </summary>
	/// <param name="combo">Combo.</param>
	public void SetCombination(string[] combo){
		weaponCombination = new string[combo.Length];
		weaponCombination = combo;
	}

	public string[] GetCombination(){
		return weaponCombination;
	}
	
	public void GiveCombination(string weaponType){
		//Debug.Log(weaponType);
		CreateCombination cc = GetComponentInChildren<CreateCombination>();
		if(cc){
			cc.GiveCustomCombo(weaponCombination);
			cc.UseCustomCombo();
		}else{
			Debug.Log("No Create Combination attached");
		}
		CreateWeaponCombination wc = GetComponentInChildren<CreateWeaponCombination>();
		if(wc){
			wc.SwapWeapon(weaponType);
		}else{
			Debug.Log("No Create Weapon Combination attached");
		}
	}
}
