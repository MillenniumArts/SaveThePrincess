using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighScoreController : MonoBehaviour {

    public int maxToDisplay = 10;
    public Text titleText;

    public Text[] scores;
    public Text[] names;

    dreamloLeaderBoard dl;
    List<dreamloLeaderBoard.Score> scoreList;

	// Use this for initialization
	void Start () {
        this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        this.dl.LoadScores();
        //this.scores = new Text[maxToDisplay];
        //this.names = new Text[maxToDisplay];
        this.titleText.text = "";
        Invoke("GetScores", 0.5f);
	}

    void GetScores()
    {
        this.scoreList = dl.ToListHighToLow();
    }

    public void ExitHighScores()
    {
        // delete player and load start menu level
        LevelLoadHandler.Instance.LoadLevel("StartMenu_LVP", true);
    }
	// Update is called once per frame
	void Update () {
        if (scoreList == null)
        {
            this.titleText.text = "Loading High scores...";
            for (int i = 0; i < maxToDisplay; i++)
            {
                this.scores[i].text = "";
                this.names[i].text = "";
            }            
        }
        else
        {
            this.titleText.text = "High Scores";
            int count = 0;
            foreach (dreamloLeaderBoard.Score curScore in scoreList)
            {
                scores[count].text = curScore.score.ToString();
                names[count].text = count+1 + ".  " + curScore.playerName.ToString();
                count++;
                // max count
                if (count >= maxToDisplay)
                    break;
            }
            if (maxToDisplay > count)
            {
                for (int i = count; i < maxToDisplay; i++)
                {
                    // set rest to null strings
                    this.scores[i].text = "";
                    this.names[i].text = "";
                }
            }
        }
	}
}
