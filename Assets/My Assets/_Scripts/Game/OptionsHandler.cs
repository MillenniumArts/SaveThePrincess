using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsHandler : MonoBehaviour {

    public Toggle muteAudio;
    public Slider audioLevel;   

	// Use this for initialization
	void Start () {
        audioLevel.maxValue = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        AudioManager.Instance.volumeFactor = audioLevel.value;
	}
}
