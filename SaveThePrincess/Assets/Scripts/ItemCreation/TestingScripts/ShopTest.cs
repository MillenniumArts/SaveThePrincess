using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopTest : MonoBehaviour {
	public Button[] buttons;
	public Button exitStore;
	public Text[] buttonText;
	public Item[] shopItems;
	public Transform spawn1, spawn2, spawn3, spawn4, spawn5, spawn6;
	private InventoryController inventory;
	private ItemFactory factory;
	private PlayerController player;
	private Vector3 prevPos;
	void Start(){
		this.player = FindObjectOfType<PlayerController>();

		this.prevPos = this.player.gameObject.transform.localPosition;
		// relocate player
		Vector3 newSpot = new Vector3 (-7.25f, -3.5f);
		this.player.gameObject.transform.localPosition = newSpot;

		for(int i = 0; i < buttonText.Length; i++){
			buttonText[i].text = "Buy";
		}
		factory = FindObjectOfType<ItemFactory>();
		inventory = FindObjectOfType<InventoryController>();
		Populateshop();

		this.exitStore.onClick.AddListener(()=>{
			ExitStore();
		});
	}

	public void ExitStore(){
		this.player.gameObject.transform.localPosition = prevPos;
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Battle_LVP");
	}

	public void BuyWeapon(int buttonNum){
		inventory.ReplaceSlot(shopItems[buttonNum], 0);
		player.CallSetWeapon(shopItems[buttonNum].GetItemSubClass());
	}
	
	public void BuyArmour(int buttonNum){
		inventory.ReplaceSlot(shopItems[buttonNum], 1);
		player.CallSetArmor(shopItems[buttonNum].GetItemSubClass());
	}
	
	private void Populateshop(){
		shopItems[0] = factory.CreateWeapon(spawn1, "Sword");
		shopItems[1] = factory.CreateWeapon(spawn2, "Hammer");
		shopItems[2] = factory.CreateWeapon(spawn3, "Dagger");
		shopItems[3] = factory.CreateArmor(spawn4, "Armor");
		shopItems[4] = factory.CreateArmor(spawn5, "Armor");
		shopItems[5] = factory.CreateArmor(spawn6, "Armor");
	}
	
}
