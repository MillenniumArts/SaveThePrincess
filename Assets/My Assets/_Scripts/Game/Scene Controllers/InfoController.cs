using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoController : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        EscapeHandler.instance.GetButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Select");
       // EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("StartMenu_LVP");
    }
}
