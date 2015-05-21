using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {
	// used to handle menu options

	public PlayerController player;
	public MenuController menuController;

	[SerializeField]
	private Button healthUp = null, 
				   healthDown= null,
				   damageUp = null,
				   damageDown = null,
				   armorUp = null, 
				   armorDown= null, 
				   confirm = null;

	private int baseHealth, baseArmor, baseDamage, 
				healthInc, armorInc, damageInc,
				newHealth, newArmor, newDamage;

	public GUIText healthAmt, armorAmt, damageAmt, creditText;
	public int numCredits;

	public ItemFactory itemFactory;


	// Use this for initialization
	void Start () {

		GameObject menuControllerObject = GameObject.FindWithTag ("MenuController");
		
		if (menuControllerObject != null) {
			menuController = menuControllerObject.GetComponent<MenuController> ();
		} 
		if (menuController == null) {
			Debug.Log("Cannot find MenuObject!");
			return;
		}
		CreateCombination[] bodies = GameObject.FindObjectsOfType<CreateCombination> ();
		// iterate through each, assigning left to left, right to right
		foreach (CreateCombination bod in bodies) {
			// get playerController 
			this.player = bod.GetComponentInParent<PlayerController> ();
		}

		this.player.body.random = false;

		// credits to upgrade character
		//this.numCredits = null; // set in editor
		// value per credit:
		this.healthInc = 10;
		this.damageInc = 5;
		this.armorInc = 2;

		// base stats
		this.baseHealth = this.player.totalHealth;
		this.baseDamage = this.player.physicalDamage;
		this.baseArmor = this.player.armor;

		// variables to be changed
		this.newHealth = this.baseHealth;
		this.newDamage = this.baseDamage;
		this.newArmor = this.baseArmor;


		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text = this.newArmor.ToString();

		this.damageAmt.text = this.newDamage.ToString ();
		this.creditText.text = "CREDITS: " + numCredits;

		// button handling - INCREASE
		this.healthUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newHealth += this.healthInc;
			}
		});
		this.damageUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newDamage += this.damageInc;
			}
		});
		this.armorUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newArmor += this.armorInc;
			}
		});

		// DECREASE STATS
		this.healthDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newHealth - this.healthInc < this.baseHealth)	// make sure they can't go below base stats
					this.newHealth = this.baseHealth;
				else{
					this.newHealth -= this.healthInc;
					numCredits++;
				}
			}
		});
		this.damageDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newDamage - this.damageInc < this.baseDamage)	// make sure they can't go below base stats
					this.newDamage = this.baseDamage;
				else{
					this.newDamage -= this.damageInc;
					numCredits++;
				}
			}
		});
		this.armorDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newArmor - this.armorInc < this.baseArmor)	// make sure they can't go below base stats
					this.newArmor = this.baseArmor;
				else{
					this.newArmor -= this.armorInc;
					numCredits++;
				}
			}
		});

		// LOAD NEXT SCENE WITH THIS PLAYER
		this.confirm.onClick.AddListener (()=>{
			if (numCredits == 0){
				this.player.totalHealth = this.newHealth;
				this.player.remainingHealth = this.newHealth;
				this.player.physicalDamage = this.newDamage;
				this.player.armor = this.newArmor;

				DontDestroyOnLoad(this.player);

				Application.LoadLevel ("Battle_LVP");
			}else{

			}
		});

	}

	void UpdateText(){
		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text =  this.newArmor.ToString();
		this.damageAmt.text = this.newDamage.ToString ();
		this.creditText.text = "CREDITS: " + numCredits;
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
}
