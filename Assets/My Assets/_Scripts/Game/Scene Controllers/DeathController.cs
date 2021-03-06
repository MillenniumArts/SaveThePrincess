using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathController : MonoBehaviour {

	public Button restartButton = null; 	// assign in editor
	public PlayerController player;

	public Text healthText, energyText, armorText, damageText, moneyText, scoreText, nameText;

    private dreamloLeaderBoard dl;

	// Use this for initialization
	void Awake () {
        this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("Death");
        EscapeHandler.instance.GetButtons();
		this.player = FindObjectOfType<PlayerController> ();
        this.player.posController.MovePlayer(35, 65);
        PlayerPrefs.SetInt("CharUnlock", PlayerPrefs.GetInt("score"));
	}

    public void Restart()
    {
        AudioManager.Instance.PlaySFX("SelectLarge");
        EnemyStats.GetInstance().ResetEnemyBaseStats();
        PlayerPrefs.SetInt("score", 0); // Reset the score to 0 for the next game.
        LevelLoadHandler.Instance.LoadLevel("StartMenu_LVP", true);
    }

    /// <summary>
    /// Saves high score to server
    /// </summary>
    public void SubmitHighScore()
    {
        if (dl.publicCode != "" && dl.privateCode != "")
        {
            if (PlayerPrefs.GetInt("score") != 0) 
            {
                AudioManager.Instance.PlaySFX("SelectSmall");
                dl.AddScore(this.player.playerName, PlayerPrefs.GetInt("score"));
            }
        }
    }

    public void LoadHighScores()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        LevelLoadHandler.Instance.LoadLevel("HighScores_LVP", false);
        this.player.posController.MovePlayer(-50, -50);
    }

	void UpdateText(){
        this.healthText.text = this.player.totalHealth.ToString();
        this.energyText.text = this.player.totalEnergy.ToString();
        this.armorText.text = this.player.GetTotalArmor().ToString();
        this.damageText.text = this.player.GetTotalDamage().ToString();
        this.moneyText.text = this.player.dollarBalance.ToString();
		this.scoreText.text = PlayerPrefs.GetInt("score").ToString();
        this.nameText.text = this.player.playerName;
	}

	// Update is called once per frame
	void Update () {
        UpdateText();
	}
}
