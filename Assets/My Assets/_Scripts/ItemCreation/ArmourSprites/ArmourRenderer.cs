using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArmourRenderer : MonoBehaviour {
    [SerializeField]
    private List<Sprite> chosenArmourSprites;
    [SerializeField]
    private string tagName;
    [SerializeField]
    private string folder, curSheetName, lastSheetName;
    [SerializeField]
    private int curSetNum/*, numOfElements*/;

    void Start()
    {
        chosenArmourSprites = new List<Sprite>();
    }

    void LateUpdate()
    {
        if (curSheetName != lastSheetName)
        {
            LoadSprites(curSetNum);
            RenderSprites();
            lastSheetName = curSheetName;
            chosenArmourSprites.Clear();
            chosenArmourSprites = new List<Sprite>();
        }
    }

    public void TransferSprites(string sheetName, int otherSetNum)
    {
        curSheetName = sheetName;
        curSetNum = otherSetNum;
    }

    private void LoadSprites(int _setNum)
    {
        Sprite[] _sprites = Resources.LoadAll<Sprite>(/*"_Final_Assets/" + */folder + "/" + curSheetName);
        for (int i = 0; i < _sprites.Length; i++)
        {
            string[] tempNames = _sprites[i].name.Split('_');
            if (tempNames[0] == _setNum.ToString())
            {
                chosenArmourSprites.Add(_sprites[i]);
            }
        }
    }

    private void RenderSprites()
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (renderer.gameObject.CompareTag(tagName))
            {
                for (int i = 0; i < chosenArmourSprites.Count; i++)
                {	// For loop search.
                    string[] spriteName = renderer.sprite.name.Split('_'); // Get the sprite name temporarily.
                    Sprite newSprite = chosenArmourSprites[i];		// Stores the current search item for comparison.
                    string[] s = newSprite.name.Split('_');
                    if (spriteName[1] == s[1])
                    {				// If the sprite renderer's sprite name = the searched sprite name.
                        renderer.sprite = newSprite;				// The searched sprite is now the sprite renderer'd new sprite.
                        break;										// Break out of the for loop search
                    }
                }
            }
        }
    }
}
