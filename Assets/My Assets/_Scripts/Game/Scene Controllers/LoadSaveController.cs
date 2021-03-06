using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadSaveController : MonoBehaviour {

    public Button loadGame, startNewGame;
    public PlayerController player;

    public void LoadGame()
    {
        SaveSystemHandler.instance.LoadGame();

        // handle proper level loading from stats
        if (SaveSystemHandler.instance.inBattle)
        {
            LevelLoadHandler.Instance.LoadLevel("Battle_LVP", false);
        }
        else
        {
            LevelLoadHandler.Instance.LoadLevel("Town_LVP", false);
        }
        AudioManager.Instance.PlaySFX("SelectSmall");
    }

    public void StartNewGame()
    {
        // start new game
        AudioManager.Instance.PlaySFX("SelectLarge");
        LevelLoadHandler.Instance.LoadLevel("CharacterSelect_LVP", false);
    }

    public void ClearSaveData()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        PlayerPrefs.SetString("GameData", "");
        PlayerPrefs.SetInt("GameToLoad", 0);
        Debug.Log(PlayerPrefs.GetInt("GameToLoad"));
    }

    #region mono
    // Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        this.player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerPrefs.GetInt("GameToLoad") == 0)
        {
            this.loadGame.gameObject.SetActive(false);
        }
    }
    #endregion mono
}
