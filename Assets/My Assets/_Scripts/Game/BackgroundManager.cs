using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundManager : MonoBehaviour {

    public int numBackgrounds;
    public string folder, spriteSheetName;
    public int backgroundNum = 0;
    //public Sprite[] backgroundImages;
    public Image background;

    public void SetBackground()
    {
        //Debug.Log("SetBackground Called");
        int newNum = PlayerPrefs.GetInt("BackgroundImageNum");
        if(newNum < numBackgrounds)
            backgroundNum = newNum;
        background.sprite = Resources.Load<Sprite>("_Final_Assets/Environments/" + folder + "/" + spriteSheetName + backgroundNum);//backgroundImages[backgroundNum];
    }

    public void BackgroundChange()
    {
        //Debug.Log("BackgroundChange Called");
        //Debug.Log("Yes BackgroundChange gets called.");
        backgroundNum = Random.Range(0, numBackgrounds);
        PlayerPrefs.SetInt("BackgroundImageNum", backgroundNum);
        //Debug.Log("PlayerPref:" + PlayerPrefs.GetInt("BackgroundImageNum"));
    }

}
