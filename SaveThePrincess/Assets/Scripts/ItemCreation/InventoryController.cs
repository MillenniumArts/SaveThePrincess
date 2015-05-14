using UnityEngine;
using System.Collections;

/// <summary>
/// Temporary Invetory controller.  Will probably change.
/// </summary>
public class InventoryController : MonoBehaviour {

	#region Variables
	/// <summary>
	/// The Item factory reference.
	/// </summary>
	public ItemFactory _itemFactory = null;

	/// <summary>
	/// The items in the inventory.
	/// 0 - Weapon
	/// 1 - Armor
	/// 2 - Magic
	/// 3 - Potion
	/// 4 - Potion
	/// </summary>
	public Item[] _items;

	/// <summary>
	/// The temporary item for copying purposes.
	/// </summary>
	public Item tempItem;

	/// <summary>
	/// The class of item that each inventory slot can hold.
	/// </summary>
	public string[] itemClasses = { "Weapon", "Armor", "Magic", "Potion", "Potion"};

	/// <summary>
	/// The number of items.
	/// </summary>
	public int _numItems;
	#endregion Variables

	#region Public Functions
	/// <summary>
	/// Replaces the slot at index provided with item provided
	/// </summary>
	/// <param name="slot">Index to out item into.</param>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot(int slot, Item item){
		StoreTempItem(item);
		Replace (slot);
	}
	/// <summary>
	/// Gets the item at provided index.
	/// </summary>
	/// <returns>The item at provided index.</returns>
	/// <param name="index">Index to get item from.</param>
	public Item getItem(int index){
		if (index < _numItems)
			return _items [index];
		else 
			return null;
	}
	
	#endregion Public Functions

	#region Private Functions
	/// <summary>
	/// Replace the item in the specified item slot with the tempItem.
	/// </summary>
	/// <param name="num">Number.</param>
	private void Replace(int num){
		if(tempItem.GetItemClass() != itemClasses[num]){
			Debug.Log("Only " + itemClasses[num] + " in this slot");
		}else{
			_items[num].SwapTo(tempItem);
			tempItem.ClearStats();
		}
	}

	/// <summary>
	/// Stores temporarily the item that will be added to the invetory.
	/// </summary>
	/// <param name="i">The item that will be added.</param>
	private void StoreTempItem(Item i){
		tempItem.SwapTo(i);
	}
	#endregion Private Functions

	#region MonoBehaviour
		
	void Start(){
		// itemFactory instance set in editor
		// set array size
		_numItems = 5;
		// temp item null until needed
		tempItem = null;
		// create array
		_items = new Item[_numItems];
		// go through array and fill with appropriate Item Children
		_items [0] = null;	// weapon
		_items [1] = null;	// armor
		_items [2] = null;	// magic
		_items [3] = null;	// potion
		_items [4] = null;	// potion
	}

	#endregion MonoBehaviour

}
