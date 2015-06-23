using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoController : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        EscapeHandler.instance.GetButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Select");
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("HowToPlay_LVP");
    }
}
