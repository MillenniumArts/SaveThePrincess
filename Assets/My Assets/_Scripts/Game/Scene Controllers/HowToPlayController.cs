using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HowToPlayController : MonoBehaviour {

    public Button backButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Select");
        Application.LoadLevel("StartMenu_LVP");
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySFX("Select");
        Application.LoadLevel("CharacterSelect_LVP");
    }
}
