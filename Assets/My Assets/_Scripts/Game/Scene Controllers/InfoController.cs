using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoController : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        EscapeHandler.instance.GetButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Button1");
       // EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("StartMenu_LVP", false);
    }
}
