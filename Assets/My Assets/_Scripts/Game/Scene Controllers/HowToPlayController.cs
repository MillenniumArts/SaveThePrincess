using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HowToPlayController : MonoBehaviour {

    public Button backButton;
	// Use this for initialization
	void Start () {
        EscapeHandler.instance.GetButtons();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoBack()
    {
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("StartMenu_LVP");
    }

    public void StartGame()
    {
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("CharacterSelect_LVP");
    }
}
