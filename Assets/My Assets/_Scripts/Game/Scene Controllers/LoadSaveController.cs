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
            EnemyStats.GetInstance().LoadNewEnemy(this.player.totalHealth, 
                                                  this.player.totalEnergy, 
                                                  this.player.GetTotalDamage(), 
                                                  this.player.GetTotalArmor()
                                                  );
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
        this.player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

    }
    #endregion mono
}
