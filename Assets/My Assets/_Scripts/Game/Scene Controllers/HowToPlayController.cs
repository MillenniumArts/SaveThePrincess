using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HowToPlayController : MonoBehaviour {

    public Button backButton;
	// Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        EscapeHandler.instance.GetButtons();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Return");
       // EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("StartMenu_LVP",false);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySFX("SelectLarge");
       // EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("StatSelect_LVP",false);
    }
}
