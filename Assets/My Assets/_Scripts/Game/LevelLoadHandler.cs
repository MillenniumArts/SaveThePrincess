using UnityEngine;
using System.Collections;

public class LevelLoadHandler : MonoBehaviour
{
    /// <summary>
    /// This class will handle level loading throughout the game
    /// </summary>

    /// <summary>
    /// 
    /// </summary>
    public string sceneToLoad;

    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
