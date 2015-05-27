using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryGUIController : InventoryController {

	public Transform[] drawLocations;
	public Button[] clickables;
	
	// Use this for initialization
	void Start () {
		this._itemFactory = FindObjectOfType<ItemFactory> ();
		this.player = FindObjectOfType<PlayerController> ();
		this.player.inventory = this;
		// set up arrays
		this._items = new Item[MAX_INVENTORY_SIZE];

		// add 5 potions to the inventory for now
		PopulateInventory ();
		ButtonInit ();

		// handle clicking
			// determine which is clicked
			// apply the effects of that Item
	}

	void ButtonInit(){
		for (int i=0; i < this.clickables.Length; i++) {
			// set sprite Images
			//Debug.Log (this.clickables[i].name);

			if (this._items [i].GetSprite() != null)
				this.clickables [i].image.sprite = this._items [i].GetSprite();
		}
	}

	public void ApplyEffect(int index){
		//this._items[index].
		Debug.Log ("Used " + index);
	}

	void PopulateInventory(){
		for (int i=0; i < MAX_INVENTORY_SIZE; i++) {
			if (i%2==0){
				this._items[i] = _itemFactory.CreatePotion(drawLocations[i]);
				this._items[i].transform.localScale = new Vector3(1.5f,1.5f,1.5f);
			}else{
				this._items[i] = _itemFactory.CreateMagic(drawLocations[i]);
				this._items[i].transform.localScale = new Vector3(1f,1f,1f);
			}
			this._items[i].transform.parent = this.drawLocations[i].transform;		// anchor to parent
		}
	}

	void OnMouseDown(){
		Debug.Log ("WOW!");
	}


	// Update is called once per frame
	void Update () {
	
	}
}
