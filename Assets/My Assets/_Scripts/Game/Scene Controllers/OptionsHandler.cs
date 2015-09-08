using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsHandler : MonoBehaviour {

    public Slider audioLevel;
    public Slider sfxLevel;
    public float musicVolumeFactor, sfxVolumeFactor;

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Return");
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("StartMenu_LVP");
    }

	// Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        audioLevel.maxValue = 1.0f;
        sfxLevel.maxValue = 1.0f;
        musicVolumeFactor = audioLevel.value;
        sfxVolumeFactor = sfxLevel.value;
	}
	
	// Update is called once per frame
	void Update () {
        musicVolumeFactor = audioLevel.value;
        sfxVolumeFactor = sfxLevel.value;
        AudioManager.Instance.volumeFactor = musicVolumeFactor;
	}
}
