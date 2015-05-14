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
				   magicUp = null, 
				   magicDown = null, 
				   speedUp = null, 
				   speedDown = null,
				   confirm = null;

	private int baseHealth, baseArmor, baseMagic, baseSpeed, baseDamage, 
				healthInc, armorInc, magicInc, speedInc, damageInc,
				newHealth, newArmor, newMagic, newSpeed, newDamage;

	public GUIText healthAmt, armorAmt, magicAmt, speedAmt, damageAmt, creditText;
	public int numCredits;


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
		this.numCredits = 10;
		// value per credit:
		this.healthInc = 10;
		this.damageInc = 5;
		this.armorInc = 2;
		this.magicInc = 2;
		this.speedInc = 1;
		// base stats
		this.baseHealth = this.player.totalHealth;
		this.baseDamage = this.player.physicalDamage;
		this.baseArmor = this.player.armor;
		this.baseMagic = this.player.magicalDamage;
		this.baseSpeed = this.player.speed;
		// variables to be changed
		this.newHealth = this.baseHealth;
		this.newDamage = this.baseDamage;
		this.newArmor = this.baseArmor;
		this.newMagic = this.baseMagic;
		this.newSpeed = this.baseSpeed;

		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text = this.newArmor.ToString();
		this.magicAmt.text = this.newMagic.ToString();
		this.speedAmt.text = this.newSpeed.ToString();
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
		this.magicUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newMagic += this.magicInc;
			}
		});
		this.speedUp.onClick.AddListener (()=>{
			if (this.numCredits > 0){
				numCredits--;
				this.newSpeed += this.speedInc;
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
		this.magicDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newMagic - this.magicInc < this.baseMagic)	// make sure they can't go below base stats
					this.newMagic = this.baseMagic;
				else{
					this.newMagic -= this.magicInc;
					numCredits++;
				}
			}
		});
		this.speedDown.onClick.AddListener (()=>{
			if (this.numCredits < 10){
				if (this.newSpeed - this.speedInc < this.baseSpeed)	// make sure they can't go below base stats
					this.newSpeed = this.baseSpeed;
				else{
					this.newSpeed -= this.magicInc;
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
				this.player.magicalDamage = this.newMagic;
				this.player.speed = this.newSpeed;

				DontDestroyOnLoad(this.player);

				Application.LoadLevel ("Testscene2");
			}else{

			}
		});

	}

	void UpdateText(){
		this.healthAmt.text = this.newHealth.ToString();
		this.armorAmt.text =  this.newArmor.ToString();
		this.magicAmt.text =  this.newMagic.ToString();
		this.speedAmt.text =  this.newSpeed.ToString();
		this.damageAmt.text = this.newDamage.ToString ();
		this.creditText.text = "CREDITS: " + numCredits;
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
}
