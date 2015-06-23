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
<<<<<<< HEAD
        AudioManager.Instance.PlaySFX("Select");
=======
        EscapeHandler.instance.ClearButtons();
>>>>>>> origin/Develop
        Application.LoadLevel("StartMenu_LVP");
    }

    public void StartGame()
    {
<<<<<<< HEAD
        AudioManager.Instance.PlaySFX("Select");
=======
        EscapeHandler.instance.ClearButtons();
>>>>>>> origin/Develop
        Application.LoadLevel("CharacterSelect_LVP");
    }
}
