using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathController : MonoBehaviour {

	public Button restartButton = null; 	// assign in editor
	public PlayerController player;

	public GUIText healthText, magicText, armorText, damageText, hiScoreText;

	// Use this for initialization
	void Start () {
		this.restartButton.onClick.AddListener (()=>{
			Application.LoadLevel("StartMenu_LVP");
		});
	
		this.healthText.text = "";
		this.armorText.text = "";
		this.magicText.text = "";
		this.damageText.text = "";
		this.hiScoreText.text = "";

		UpdateText ();

	}

	void UpdateText(){
		this.healthText.text = "HEALTH: " + this.player.totalHealth;
		
		this.magicText.text = "MAGIC: " + this.player.magicalDamage;
		
		this.armorText.text = "ARMOR: " + this.player.armor;
		
		this.damageText.text = "DAMAGE: " + this.player.physicalDamage;
		
		this.hiScoreText.text = "HI SCORE:" + PlayerPrefs.GetInt("hiscore");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
