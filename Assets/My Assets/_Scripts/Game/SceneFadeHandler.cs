using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeHandler : MonoBehaviour {

    public float alpha = 1.0f;
    public Image fadeImg;
    public bool levelStarting = true;


    public void FadeToBlack()
    {
        alpha+=0.01f;
    }

    public void FadeToClear()
    {
        alpha -= 0.01f;
    }

	// Use this for initialization
	void Start () {
        alpha = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        // limit to 0-1
        alpha = Mathf.Clamp01(alpha);

        Color temp = new Color(0, 0, 0, alpha);
        fadeImg.color = temp;

        if (levelStarting)
            FadeToClear();
        else
            FadeToBlack();

        if (alpha < 0.0f || alpha > 1.0f)
        {
            this.fadeImg.gameObject.SetActive(false);
        }
        else
        {
            this.fadeImg.gameObject.SetActive(true);
        }

	}

    #region Singleton
    private static SceneFadeHandler _instance;

    public static SceneFadeHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneFadeHandler>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }
    #endregion singleton
}
