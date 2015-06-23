using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoController : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX("Select");
        Application.LoadLevel("HowToPlay_LVP");
    }
}
