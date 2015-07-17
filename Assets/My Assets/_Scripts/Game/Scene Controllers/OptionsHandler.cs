using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsHandler : MonoBehaviour {

    public Slider audioLevel;
    public float volumeFactor;

    public void GoBack()
    {
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("StartMenu_LVP");
    }

	// Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        audioLevel.maxValue = 1.0f;
        volumeFactor = audioLevel.value;
	}
	
	// Update is called once per frame
	void Update () {
        volumeFactor = audioLevel.value;
        AudioManager.Instance.volumeFactor = volumeFactor;
	}
}
