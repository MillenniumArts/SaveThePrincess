using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NewWeaponSprites : MonoBehaviour
{
    [SerializeField]
    private string folder, spriteSheetName;
    [SerializeField]
    private SpriteRenderer[] heads, guards;
    [SerializeField]
    private List<Sprite> _Heads, _Guards;
    [SerializeField]
    private Sprite chosenHead, chosenGuard;

    void Start()
    {
        _Heads = new List<Sprite>();    // Creates the lists.
        _Guards = new List<Sprite>();   //
    }

    /// <summary>
    /// Swaps the sprites of the weapon with the sprite of the new weapon.
    /// </summary>
    /// <param name="sheetName">Name for the new Sprite Sheet.</param>
    public void SwapSprites(string sheetName)
    {
        SetSpriteSheetName(sheetName);                      // Sets the new sprite sheet name.
        LoadSprites();                                      // Loads the new sprites.
        int rand1 = RandomNumberGenerator(_Heads.Count);    // Gets a random number to pick the head of the weapon.
        int rand2 = RandomNumberGenerator(_Guards.Count);   // Gets a random number to pick the guard of the weapon.
        SetSprites(heads, _Heads, rand1, chosenHead);                   // Sets the weapon head.
        SetSprites(guards, _Guards, rand2, chosenGuard);                 // Sets the weapon guard
        chosenHead = _Heads[rand1];
        chosenGuard = _Guards[rand2];
        _Heads.Clear();                                     // Clears the sprite list.
        _Guards.Clear();                                    // Clears the sprite list.
        _Heads = new List<Sprite>();                        // Creates a new list.
        _Guards = new List<Sprite>();                       // Creates a new list.
    }

    public Sprite GetChosenHead()
    {
        return chosenHead;
    }

    public Sprite GetChosenGuard()
    {
        return chosenGuard;
    }

    public void SetNewSprites(NewWeaponSprites nWS)
    {
        chosenHead = nWS.GetChosenHead();
        chosenGuard = nWS.GetChosenGuard();
        for(int i = 0; i < heads.Length; i++){
            heads[i].sprite = chosenHead;
            guards[i].sprite = chosenGuard;
        }
    }

    #region Private Methods
    private void SetSpriteSheetName(string _sheetName)
    {
        spriteSheetName = _sheetName;
    }

    private void LoadSprites()
    {
        Sprite[] _sprites = Resources.LoadAll<Sprite>("_Final_Assets/" + folder + "/" + spriteSheetName);
        for (int i = 0; i < _sprites.Length; i++)
        {
            string[] tempNames = _sprites[i].name.Split('_');
            if (tempNames[0] == "Head")
            {
                _Heads.Add(_sprites[i]);
            }
            else
            {
                _Guards.Add(_sprites[i]);
            }
        }
    }

    private int RandomNumberGenerator(int MAX)
    {
        int r = Random.Range(0, MAX);
        return r;
    }

    private void SetSprites(SpriteRenderer[] rendererArray, List<Sprite> spriteList, int index, Sprite chosenSprite)
    {
        for (int i = 0; i < rendererArray.Length; i++)
        {
            rendererArray[i].sprite = spriteList[index];
        }
    }

    private void SetSprites(SpriteRenderer[] rendererArray, Sprite chosenSprite)
    {
        Debug.Log(chosenSprite);
        for (int i = 0; i < rendererArray.Length; i++)
        {
            rendererArray[i].sprite = chosenSprite;
        }
    }
    #endregion Private Methods

    #region Testing
    /*public bool go;
    void Update()
    {
        if (go)
        {
            SwapSprites(spriteSheetName);
            go = false;
        }
    }*/
    #endregion Testing
}

