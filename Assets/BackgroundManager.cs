using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundManager : MonoBehaviour {

    public int numBackgrounds;
    public int backgroundNum = 0;
    public Sprite[] backgroundImages;
    public Image background;

    public void SetBackground()
    {
        int newNum = PlayerPrefs.GetInt("BackgroundImageNum");
        if (newNum != null)
        {
            backgroundNum = newNum;
        }
        background.sprite = backgroundImages[backgroundNum];
    }

    public void BackgroundChange()
    {
        //Debug.Log("Yes BackgroundChange gets called.");
        backgroundNum = Random.Range(0, numBackgrounds);
        PlayerPrefs.SetInt("BackgroundImageNum", backgroundNum);
        //Debug.Log("PlayerPref:" + PlayerPrefs.GetInt("BackgroundImageNum"));
    }

}
