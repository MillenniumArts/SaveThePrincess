using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryMenuController : MonoBehaviour {
	public PlayerController player;
	public InventoryMenuController inventoryMenu;

	public Button useSlot1 = null,
	useSlot2 = null,
	useSlot3 = null;
	
	public GUIText healthText, manaText;


	// Use this for initialization
	void Start () {

		// Player should still exist from battle scene		
		GameObject menuControllerObject = GameObject.FindWithTag ("GameController");
		if (menuControllerObject != null) {
			inventoryMenu = menuControllerObject.GetComponent<InventoryMenuController> ();
		} 
		if (menuControllerObject == null) {
			Debug.Log("Cannot find GameObject!");
			return;
		}
		// get play er from scene
		// grab all CreateCombinations
		CreateCombination[] bodies = GameObject.FindObjectsOfType<CreateCombination> ();
		
		// iterate through each, assigning left to left, right to right
		foreach (CreateCombination bod in bodies) {
			// get playerController 
			PlayerController pc = bod.GetComponentInParent<PlayerController> ();
			// check for left or Right
			if (pc.tag == "Player") {
				// assign PC
				this.player = pc;
			} 
		}

		// initialize the buttons
		useSlot1.onClick.AddListener (()=>{
			// apply buff attached to that inventory item

			// JUST STUPID HARDCODED FOR NOW
			if (this.player.remainingHealth + 10 <= this.player.totalHealth)
				this.player.remainingHealth += 10;
			else
				this.player.remainingHealth = this.player.totalHealth;

			// return to battle scene
			ReturnToBattle();
		});
		useSlot2.onClick.AddListener (()=>{
			// apply buff attached to that inventory item
			// return to battle scene
			ReturnToBattle();
		});
		useSlot3.onClick.AddListener (()=>{
			// apply buff attached to that inventory item
			// return to battle scene
			ReturnToBattle();
		});
	}

	void ReturnToBattle(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Battle_LVP");
	}

	void UpdateText(){
		healthText.text = "HEALTH : " + this.player.remainingHealth + "/" + this.player.totalHealth;
		manaText.text = "MANA : " + this.player.remainingMana + "/" + this.player.totalMana;
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
}
