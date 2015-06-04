using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryGUIController : InventoryController {

	public Transform[] drawLocations;
	public Button[] clickables = null;
	public Text[] buttonText = null;
	public bool initialized;
	
	// Use this for initialization
	void Start () {
		initialized = false;
	}
	
	public void PopulateInventory(){
		this.initialized = true;
		this._items = new Item[MAX_INVENTORY_SIZE];
		this._itemFactory = FindObjectOfType<ItemFactory> ();

		for (int i=0; i < MAX_INVENTORY_SIZE; i++) {
			if (i % 2 == 0) {
				this._items [i] = _itemFactory.CreatePotion (drawLocations [i], "HealPotion");
				this._items [i].transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
			} else if (i == 1) {
				this._items [i] = _itemFactory.CreateMagic (drawLocations [i], "AttackMagic");
				this._items [i].transform.localScale = new Vector3 (0.5f, 0.5f);
			} else if (i == 3) {
				this._items [i] = _itemFactory.CreateMagic (drawLocations [i], "HealMagic");
				this._items [i].transform.localScale = new Vector3 (0.5f, 0.5f);
			}
			this._items [i].transform.parent = this.drawLocations [i].transform;		// anchor to parent
		}
	}

	public void DisableButtonsIfUsed(){

		for (int i = 0; i < this._items.Length; i++) {
			if (this._items[i].used){
				this.clickables[i].gameObject.SetActive(false);
				this._items[i].gameObject.SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	}
}
