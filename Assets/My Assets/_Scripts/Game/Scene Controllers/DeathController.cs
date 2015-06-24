using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathController : MonoBehaviour {

	public Button restartButton = null; 	// assign in editor
	public PlayerController player;

	public Text healthText, energyText, armorText, damageText, moneyText, scoreText;

	// Use this for initialization
	void Start () {
        EscapeHandler.instance.GetButtons();
		this.player = FindObjectOfType<PlayerController> ();

		this.restartButton.onClick.AddListener (()=>{
			Destroy(this.player);
            EscapeHandler.instance.ClearButtons();
            EnemyStats.GetInstance().ResetEnemyBaseStats();
			Application.LoadLevel("StartMenu_LVP");
		});
	
		this.healthText.text = "";
		this.armorText.text = "";
		this.energyText.text = "";
		this.damageText.text = "";
		this.scoreText.text = "";
		this.moneyText.text = "";

		UpdateText ();

	}

	void UpdateText(){
        this.healthText.text = this.player.totalHealth.ToString();

        this.energyText.text = this.player.totalEnergy.ToString();

        this.armorText.text = this.player.armor.ToString();

        this.damageText.text = this.player.physicalDamage.ToString();

        this.moneyText.text = this.player.dollarBalance.ToString();
		
		this.scoreText.text = PlayerPrefs.GetInt("score").ToString();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
