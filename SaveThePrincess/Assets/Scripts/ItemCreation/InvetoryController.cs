using UnityEngine;
using System.Collections;

/// <summary>
/// Temporary Invetory controller.  Will probably change.
/// </summary>
public class InvetoryController : MonoBehaviour {

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
	/// The class of item that each inventory slot can hold.
	/// </summary>
	public string[] itemClasses = {"Weapon", "Armor", "Magic", "Potion", "Potion"};
	#endregion Variables

	#region Public Functions
	/// <summary>
	/// Replaces the slot1.
	/// </summary>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot1(Item item){
		StoreTempItem(item);
		Replace (0);
	}

	/// <summary>
	/// Replaces the slot2.
	/// </summary>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot2(Item item){
		StoreTempItem(item);
		Replace(1);
	}

	/// <summary>
	/// Replaces the slot3.
	/// </summary>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot3(Item item){
		StoreTempItem(item);
		Replace (2);
	}

	/// <summary>
	/// Replaces the slot4.
	/// </summary>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot4(Item item){
		StoreTempItem(item);
		Replace (3);
	}

	/// <summary>
	/// Replaces the slot5.
	/// </summary>
	/// <param name="item">Item to be put in the slot.</param>
	public void ReplaceSlot5(Item item){
		StoreTempItem(item);
		Replace (4);
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

}
