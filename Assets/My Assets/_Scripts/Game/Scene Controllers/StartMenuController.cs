using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuController : MonoBehaviour {
    
	public Button startButton = null,
				  resetHiScore = null,
				  resetScore = null;
	public Text hiScoreText, resetScoreText;

	// Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        EscapeHandler.instance.GetButtons();
        PlayerPrefs.SetInt("score", 0);
        AudioManager.Instance.PlayNewSong("Main_Menu");
	}

    void Awake()
    {
        PlayerPrefs.SetInt("midgame", 0);
        // reset to level 1
        DifficultyLevel.GetInstance().ResetDifficulty();
        // make sure we start at 0
        BattleCounter.GetInstance().ResetCurrentBattleCount();
        BattleCounter.GetInstance().ResetBattlesNeeded();
        // set number of battles
        BattleCounter.GetInstance().SetBattlesNeeded(DifficultyLevel.GetInstance().GetDifficultyMultiplier());
    }

	public void StartGame(){
        AudioManager.Instance.PlaySFX("SelectLarge");
        //EscapeHandler.instance.ClearButtons();
		LevelLoadHandler.Instance.LoadLevel("LoadSave_LVP", false);
	}

    public void HowToPlay()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("HowToPlay_LVP", false);
    }

    public void Info()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Options_LVP", false);
    }

    public void Credits()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("InfoScene_LVP", false);
    }

	public void ResetHiScore(){
        AudioManager.Instance.PlaySFX("SelectSmall");
        PlayerPrefs.SetInt("hiscore", 0);
	}

	public void ResetScore(){
        AudioManager.Instance.PlaySFX("SelectSmall");
		PlayerPrefs.SetInt ("score", 0);
	}
    
	private void GetScore(){
		this.hiScoreText.text = "High Score: " + PlayerPrefs.GetInt("hiscore").ToString ();
		this.resetScoreText.text = "Reset Score: " + PlayerPrefs.GetInt ("score").ToString ();
	}

	// Update is called once per frame
	void Update () {
		GetScore ();
	}
}
