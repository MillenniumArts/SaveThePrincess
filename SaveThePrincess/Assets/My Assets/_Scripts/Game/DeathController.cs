using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathController : MonoBehaviour {

	public Button restartButton = null; 	// assign in editor
	public PlayerController player;

	public Text healthText, magicText, armorText, damageText, moneyText, hiScoreText;

	// Use this for initialization
	void Start () {

		this.player = FindObjectOfType<PlayerController> ();

		this.restartButton.onClick.AddListener (()=>{
			Destroy(this.player);
			Application.LoadLevel("StartMenu_LVP");
		});
	
		this.healthText.text = "";
		this.armorText.text = "";
		this.magicText.text = "";
		this.damageText.text = "";
		this.hiScoreText.text = "";
		this.moneyText.text = "";

		UpdateText ();

	}

	void UpdateText(){
		this.healthText.text = "HEALTH: " + this.player.totalHealth;
		
		this.magicText.text = "MAGIC: " + this.player.magicalDamage;
		
		this.armorText.text = "ARMOR: " + this.player.armor;
		
		this.damageText.text = "DAMAGE: " + this.player.physicalDamage;

		this.moneyText.text = "MONEY: " + this.player.dollarBalance;
		
		this.hiScoreText.text = "HI SCORE: " + PlayerPrefs.GetInt("hiscore");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
