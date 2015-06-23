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
            AudioManager.Instance.PlaySFX("Select");
			Destroy(this.player);
<<<<<<< HEAD
            AudioManager.Instance.PlayNewSong("ForestOverworld");
			Application.LoadLevel("StartMenu_LVP");
=======
            EscapeHandler.instance.ClearButtons();
>>>>>>> origin/Develop
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
        AudioManager.Instance.PlayNewSong("Death");
	}

	void UpdateText(){
		this.healthText.text = "HEALTH: " + this.player.totalHealth;
		
		this.energyText.text = "ENERGY:  " + this.player.totalEnergy;
		
		this.armorText.text = "ARMOR: " + this.player.armor;
		
		this.damageText.text = "DAMAGE: " + this.player.physicalDamage;

		this.moneyText.text = "MONEY: " + this.player.dollarBalance;
		
		this.scoreText.text = "SCORE: " + PlayerPrefs.GetInt("score");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
