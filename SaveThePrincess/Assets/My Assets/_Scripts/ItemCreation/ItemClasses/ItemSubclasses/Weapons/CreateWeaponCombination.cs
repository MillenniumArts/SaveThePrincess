﻿using UnityEngine;
using System.Collections;

public class CreateWeaponCombination : MonoBehaviour {
	public SpriteRenderer[] sprites;
	public bool swapNow = false;
	private bool[] onOff;
	public bool dagger, hammer, axe, sword, spear;
	
	void Start(){
		onOff = new bool[11];
	}
	
	void Update(){
		if(swapNow){
			if(dagger){
				IsDagger();
                sprites[5].sortingLayerName = "Weapon1";
                sprites[6].sortingLayerName = "Weapon1";
			}else if(hammer){
				IsHammer();
                sprites[5].sortingLayerName = "Weapon2";
                sprites[6].sortingLayerName = "Weapon2";
			}else if(axe){
				IsAxe();
                sprites[5].sortingLayerName = "Weapon2";
                sprites[6].sortingLayerName = "Weapon2";
			}else if(sword){
				IsSword();
                sprites[5].sortingLayerName = "Weapon1";
                sprites[6].sortingLayerName = "Weapon1";
			}else if(spear){
				IsSpear();
                sprites[5].sortingLayerName = "Weapon2";
                sprites[6].sortingLayerName = "Weapon2";
			}else{
				IsOff();
			}
			for(int i = 0; i < sprites.Length; i++){
				sprites[i].enabled = onOff[i];
			}
			swapNow = false;
		}
	}
	
	public void SwapNow(){
		swapNow = true;
	}
	
	public void AllOff(){
		dagger = axe = hammer = sword = spear = false;
	}
	
	public void SwapWeapon(string name){
		AllOff();
		switch(name){
		case "Dagger": dagger = true;
			break;
		case "Axe": axe = true;
			break;
		case "Hammer": hammer = true;
			break;
		case "Sword": sword = true;
			break;
		case "Spear": spear = true;
			break;
		default:
			Debug.Log("No weapon selected.  Or mispelled");
			break;
		}
		SwapNow();
	}
	
	public void IsDagger(){
		onOff[0] = true;
		onOff[1] = false;
		onOff[2] = false;
		onOff[3] = false;
		onOff[4] = false;
		onOff[5] = false;
		onOff[6] = true;
		onOff[7] = false;
		onOff[8] = true;
		onOff[9] = false;
		onOff[10] = false;
	}
	
	public void IsAxe(){
		onOff[0] = false;
		onOff[1] = false;
		onOff[2] = true;
		onOff[3] = false;
		onOff[4] = false;
		onOff[5] = true;
		onOff[6] = true;
		onOff[7] = true;
		onOff[8] = false;
		onOff[9] = false;
		onOff[10] = false;
	}
	
	public void IsHammer(){
		onOff[0] = false;
		onOff[1] = true;
		onOff[2] = false;
		onOff[3] = false;
		onOff[4] = false;
		onOff[5] = true;
		onOff[6] = true;
		onOff[7] = true;
		onOff[8] = false;
		onOff[9] = false;
		onOff[10] = false;
	}
	
	public void IsSword(){
		onOff[0] = false;
		onOff[1] = false;
		onOff[2] = false;
		onOff[3] = true;
		onOff[4] = false;
		onOff[5] = false;
		onOff[6] = true;
		onOff[7] = false;
		onOff[8] = true;
		onOff[9] = false;
		onOff[10] = false;
	}
	
	public void IsSpear(){
		onOff[0] = false;
		onOff[1] = false;
		onOff[2] = false;
		onOff[3] = false;
		onOff[4] = true;
		onOff[5] = true;
		onOff[6] = true;
		onOff[7] = true;
		onOff[8] = false;
		onOff[9] = false;
		onOff[10] = true;
	}
	
	public void IsOff(){
		onOff[0] = false;
		onOff[1] = false;
		onOff[2] = false;
		onOff[3] = false;
		onOff[4] = false;
		onOff[5] = false;
		onOff[6] = false;
		onOff[7] = false;
		onOff[8] = false;
		onOff[9] = false;
		onOff[10] = false;
	}
}