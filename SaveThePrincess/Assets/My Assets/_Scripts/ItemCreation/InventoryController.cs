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
	public ItemFactory _itemFactory;

	/// <summary>
	/// The items in the inventory.
	/// </summary>
	public Item[] _items;

	/// <summary>
	/// The temporary item for copying purposes.
	/// </summary>
	public Item tempItem;

	/// <summary>
	/// The MAX Inventory Size.
	/// </summary>
	public int MAX_INVENTORY_SIZE = 5;

	#endregion Variables

	#region Public Functions

	/// <summary>
	/// Replaces the slot1.
	/// </summary>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot(Item item, int index){
		StoreTempItem(item);
		Replace (index);
	}
	
	#endregion Public Functions

	#region Private Functions
	/// <summary>
	/// Replace the item in the specified item slot with the tempItem.
	/// </summary>
	/// <param name="num">Number.</param>
	private void Replace(int num){
			_items[num].SwapTo(tempItem);
			tempItem.ClearStats();
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


	}

	void Update(){

	}
	#endregion MonoBehaviour
}
