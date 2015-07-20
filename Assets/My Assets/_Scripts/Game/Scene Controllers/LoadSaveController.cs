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
            LevelLoadHandler.Instance.LoadLevel("Battle_LVP");
        }
        else
        {
            LevelLoadHandler.Instance.LoadLevel("Town_LVP");
        }
    }

    public void StartNewGame()
    {
        // start new game
        LevelLoadHandler.Instance.LoadLevel("CharacterSelect_LVP");
    }

    #region mono
    // Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        this.player = FindObjectOfType<PlayerController>();
        if (PlayerPrefs.GetInt("GameToLoad") == 0)
        {
            this.loadGame.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

    }
    #endregion mono
}
