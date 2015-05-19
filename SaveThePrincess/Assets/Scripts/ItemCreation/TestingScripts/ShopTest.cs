using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopTest : MonoBehaviour {
	public Text[] buttons;
	public Item[] storeItems;
	public Transform spawn1, spawn2, spawn3;
	private InventoryController inventory;
	private ItemFactory factory;
	void Start(){
		for(int i = 0; i < buttons.Length; i++){
			buttons[i].text = "Buy";
		}
		factory = FindObjectOfType<ItemFactory>();
		inventory = FindObjectOfType<InventoryController>();
		PopulateStore();
	}

	public void BuyItem1(){
		inventory.ReplaceSlot1(storeItems[0]);
	}

	public void BuyItem2(){
		inventory.ReplaceSlot1(storeItems[1]);
	}

	public void BuyItem3(){
		inventory.ReplaceSlot1(storeItems[2]);
	}

	private void PopulateStore(){
		storeItems[0] = factory.CreateWeapon(spawn1);
		storeItems[1] = factory.CreateWeapon(spawn2);
		storeItems[2] = factory.CreateWeapon(spawn3);
	}

}
