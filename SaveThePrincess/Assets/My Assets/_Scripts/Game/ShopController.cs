using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	public Button[] buttons;
	public Button exitStore;
	public Text[] buttonText;
	public Text playerBalance;
	public Item[] shopItems;
	public Transform spawn1, spawn2, spawn3, spawn4, spawn5, spawn6;
	public int HI_DOLLAR_VALUE;
	public int LO_DOLLAR_VALUE;

	private ItemFactory factory;
	private PlayerController player;
	private Vector3 prevPos;

	private bool start;

	void Start(){
		start = true;
		this.player = FindObjectOfType<PlayerController>();
		this.prevPos = this.player.gameObject.transform.localPosition;

		// relocate player
		Vector3 newSpot = new Vector3 (-7.25f, -3.5f);
		this.player.gameObject.transform.localPosition = newSpot;
		this.player.inventory.gameObject.SetActive (false);

		this.playerBalance.text = this.player.dollarBalance.ToString ();
		
		factory = FindObjectOfType<ItemFactory>();
		// min and max for cost rand
		this.LO_DOLLAR_VALUE = 15;
		this.HI_DOLLAR_VALUE = Mathf.FloorToInt(PlayerPrefs.GetInt ("score")*5.5f) + this.LO_DOLLAR_VALUE;

		PopulateShop();

	}

	private void DoOnFirstTick(){
		if(start){
			for(int i = 0; i < buttonText.Length; i++){
				buttonText[i].text = "$" + shopItems [i].dollarCost + "\n" + shopItems[i].GetName();
			}
			start = false;
		}
	}

	public void ExitStore(){
		this.player.gameObject.transform.localPosition = prevPos;
		this.player.inventory.gameObject.SetActive (true);
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Battle_LVP");
	}

	public void BuyWeapon(int buttonNum){
		if (this.player.PurchaseItem (shopItems [buttonNum].dollarCost)) {
			this.player.playerWeapon = (Weapon)shopItems [buttonNum];
			//inventory.ReplaceSlot (shopItems [buttonNum], 0);
			player.TransferPurchasedWeapon(shopItems[buttonNum]);
			player.CallSetWeapon(this.player.playerWeapon.GetItemSubClass());
		}
	}

	//
	public void BuyArmour(int buttonNum){
		if (this.player.PurchaseItem (shopItems [buttonNum].dollarCost)) {
			this.player.playerArmor = (Armor)shopItems [buttonNum];
			//inventory.ReplaceSlot (shopItems [buttonNum], 1);
			player.CallSetArmor (this.player.playerArmor.GetItemSubClass());

		}
	}
	
	private void PopulateShop(){
		shopItems[0] = factory.CreateWeapon(spawn1, "Sword");
		shopItems[1] = factory.CreateWeapon(spawn2, "Hammer");
		shopItems[2] = factory.CreateWeapon(spawn3, "Spear");
		shopItems[3] = factory.CreateArmor(spawn4, "Armor");
		shopItems[4] = factory.CreateArmor(spawn5, "Armor");
		shopItems[5] = factory.CreateArmor(spawn6, "Armor");

		// randomize cost
		for (int i=0; i < shopItems.Length; i++) {
			shopItems[i].dollarCost = Random.Range (LO_DOLLAR_VALUE, HI_DOLLAR_VALUE);
		}
	}

	public void UpdateText(){
		this.playerBalance.text = "Remaining Balance: $" + this.player.dollarBalance.ToString ();
	}

	public void Update(){
		DoOnFirstTick();
		UpdateText ();
	}
}
