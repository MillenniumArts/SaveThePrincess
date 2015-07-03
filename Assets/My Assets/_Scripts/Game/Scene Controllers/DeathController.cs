using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathController : MonoBehaviour {

	public Button restartButton = null; 	// assign in editor
	public PlayerController player;

	public Text healthText, energyText, armorText, damageText, moneyText, scoreText;

	// Use this for initialization
	void Start () {
        AudioManager.Instance.PlayNewSong("Death");
        EscapeHandler.instance.GetButtons();
		this.player = FindObjectOfType<PlayerController> ();

		this.restartButton.onClick.AddListener (()=>{
            AudioManager.Instance.PlaySFX("Select");
            EnemyStats.GetInstance().ResetEnemyBaseStats();
            //EscapeHandler.instance.ClearButtons();
			Destroy(this.player);
            LevelLoadHandler.Instance.LoadLevel("StartMenu_LVP");
		});
	
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
