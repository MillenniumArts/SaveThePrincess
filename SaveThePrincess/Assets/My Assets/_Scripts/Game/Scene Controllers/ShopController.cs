﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	public Button[] buttons;
	public Button buyButton;
	public Button exitStore;
	public Text[] buttonText;
	public Text playerBalance;
	public Item[] shopItems;
	public int selectedItem = -1;
	public Transform spawn1, spawn2, spawn3, spawn4, spawn5, spawn6;
	public int HI_DOLLAR_VALUE;
	public int LO_DOLLAR_VALUE;

	public Text currentStatDisplay;
	public Text selectedItemStats;

	private ItemFactory factory;
	private PlayerController player;
	private Vector3 prevPos;

	//private bool start;

	void Start(){
		//start = true;
		this.player = FindObjectOfType<PlayerController>();
		this.prevPos = this.player.gameObject.transform.localPosition;

		// relocate player
		Vector3 newSpot = new Vector3 (-5.7f, -2f);
		this.player.gameObject.transform.localPosition = newSpot;

		if(player.inventory)
			this.player.inventory.gameObject.SetActive (false);

		this.playerBalance.text = this.player.dollarBalance.ToString ();
		
		factory = FindObjectOfType<ItemFactory>();
		// min and max for cost rand
		this.LO_DOLLAR_VALUE = 15;
		this.HI_DOLLAR_VALUE = (int)((Mathf.FloorToInt(PlayerPrefs.GetInt ("score")*2.0f) / 2) + this.LO_DOLLAR_VALUE);

		PopulateShop();

	}

	private void ItemNameUpdate(){
		for(int i = 0; i < buttonText.Length; i++){
			buttonText[i].text = "$" + shopItems [i].dollarCost + "\n" + shopItems[i].GetName();
		}
	}

	public void ExitStore(){
		this.player.gameObject.transform.localPosition = prevPos;
		//if(player.inventory != null)
		//	this.player.inventory.gameObject.SetActive (true);
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Town_LVP");
	}

	public void SelectItem(int buttonNum){
		selectedItemStats.text = shopItems[buttonNum].GetStatsString();
		selectedItem = buttonNum;
		buyButton.enabled = true;
		buyButton.image.color = Color.white;

		// if player cant afford item
		if (player.dollarBalance < shopItems [buttonNum].GetDollarCost ()) {
			// turn button red
			buyButton.image.color = Color.red;
		} else if (player.dollarBalance >= shopItems [buttonNum].GetDollarCost ()) {
			// turn button gren
			buyButton.image.color = Color.green;
		}
	}
	
	public void BuyItem(){
		if (this.player.PurchaseItem (shopItems [selectedItem].dollarCost)) {	// can afford
			if(shopItems[selectedItem].GetItemClass() == "Armor"){				// Armor
				player.TransferPurchasedArmor(shopItems[selectedItem]);
			}else if(shopItems[selectedItem].GetItemClass() == "Weapon"){		// Weapon
				player.TransferPurchasedWeapon(shopItems[selectedItem]);
			}else if (shopItems[selectedItem].GetItemClass() == "Potion"){		// Potion
				
			}else if (shopItems[selectedItem].GetItemClass() == "Magic"){		// Magic
				
			}else{															// Other
				Debug.Log (shopItems[selectedItem].GetItemClass() + " is not a recognized Item Class!");
			}
			DestroyItem(selectedItem);
		}
		selectedItemStats.text = "";//shopItems[selectedItem].GetStatsString();
		selectedItem = -1;
	}

	private void DestroyItem(int n){
		Destroy(buttons[n].gameObject);
		Destroy (shopItems[n].gameObject);
	}
	
	private void PopulateShop(){
		shopItems[0] = factory.CreateWeapon(spawn1, "Sword");
		shopItems[0].transform.parent = spawn1.transform;
		shopItems[1] = factory.CreateWeapon(spawn2, "Hammer");
		shopItems[1].transform.parent = spawn2.transform;
		shopItems[2] = factory.CreateWeapon(spawn3, "Spear");
		shopItems[2].transform.parent = spawn3.transform;
		shopItems[3] = factory.CreateArmor(spawn4, "Armor");
		shopItems[3].transform.parent = spawn4.transform;
		shopItems[4] = factory.CreateArmor(spawn5, "Armor");
		shopItems[4].transform.parent = spawn5.transform;
		shopItems[5] = factory.CreateArmor(spawn6, "Armor");
		shopItems[5].transform.parent = spawn6.transform;

		// randomize cost
		for (int i=0; i < shopItems.Length; i++) {
			shopItems[i].dollarCost = Random.Range (LO_DOLLAR_VALUE, HI_DOLLAR_VALUE);
		}
	}

	public void UpdateText(){
		this.playerBalance.text = "Remaining Balance: $" + this.player.dollarBalance.ToString ();
		this.currentStatDisplay.text = "Weapon: \n" + player.playerWeapon.GetName() + "\n" +
			"DMG: " + player.playerWeapon.GetAtkMod() + " | " + 
			"AMR: " + player.playerWeapon.GetDefMod() + "\n" +
			"Armor: \n" + player.playerArmor.GetName() + "\n" +
			"DMG: " +  player.playerArmor.GetAtkMod() + " | " + 
			"AMR: " +  player.playerArmor.GetDefMod() ;
	}

	public void Update(){
		ItemNameUpdate();
		UpdateText ();
		if(selectedItem < 0){
			buyButton.enabled = false;
		}
	}
}
