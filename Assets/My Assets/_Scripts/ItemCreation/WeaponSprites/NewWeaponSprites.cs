using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NewWeaponSprites : MonoBehaviour
{
    [SerializeField]
    private string folder, spriteSheetName, handleSheetName;
    [SerializeField]
    private SpriteRenderer[] heads, guards, handles, pommels;
    [SerializeField]
    private List<Sprite> _Heads, _Guards, _Handles, _Pommels;
    [SerializeField]
    private Sprite chosenHead, chosenGuard, chosenHandle, chosenPommel;

    void Start()
    {
        _Heads = new List<Sprite>();    // Creates the lists.
        _Guards = new List<Sprite>();   //
        _Handles = new List<Sprite>();  //
        _Pommels = new List<Sprite>();  //
    }

    /// <summary>
    /// Swaps the sprites of the weapon with the sprite of the new weapon.
    /// </summary>
    /// <param name="sheetName">Name for the new Sprite Sheet.</param>
    public void SwapSprites(string sheetName)
    {
        SetSpriteSheetName(sheetName);                      // Sets the new sprite sheet name.
        LoadSprites();                                      // Loads the new sprites.
        LoadHandles();
        int rand1 = RandomNumberGenerator(_Heads.Count);    // Gets a random number to pick the head of the weapon.
        int rand2 = RandomNumberGenerator(_Guards.Count);   // Gets a random number to pick the guard of the weapon.
        int rand3 = RandomNumberGenerator(_Handles.Count);
        int rand4 = RandomNumberGenerator(_Pommels.Count);
        SetSprites(heads, _Heads, rand1, chosenHead);                   // Sets the weapon head.
        SetSprites(guards, _Guards, rand2, chosenGuard);                 // Sets the weapon guard
        SetSprites(handles, _Handles, rand3, chosenHandle);
        SetSprites(pommels, _Pommels, rand4, chosenPommel);
        chosenHead = _Heads[rand1];
        chosenGuard = _Guards[rand2];
        chosenHandle = _Handles[rand3];
        chosenPommel = _Pommels[rand4];
        _Heads.Clear();                                     // Clears the sprite list.
        _Guards.Clear();                                    // Clears the sprite list.
        _Handles.Clear();
        _Pommels.Clear();
        _Heads = new List<Sprite>();                        // Creates a new list.
        _Guards = new List<Sprite>();                       // Creates a new list.
        _Handles = new List<Sprite>();                      // Creates a new list.
        _Pommels = new List<Sprite>();                      // Creates a new list.
    }

    public Sprite GetChosenHead()
    {
        return chosenHead;
    }

    public Sprite GetChosenGuard()
    {
        return chosenGuard;
    }

    public Sprite GetChosenHandle()
    {
        return chosenHandle;
    }

    public Sprite GetChosenPommel()
    {
        return chosenPommel;
    }

    public void SetNewSprites(NewWeaponSprites nWS)
    {
        chosenHead = nWS.GetChosenHead();
        chosenGuard = nWS.GetChosenGuard();
        chosenHandle = nWS.GetChosenHandle();
        chosenPommel = nWS.GetChosenPommel();
        for(int i = 0; i < heads.Length; i++){
            heads[i].sprite = chosenHead;
            guards[i].sprite = chosenGuard;
        }
        for(int i = 0; i < handles.Length; i++)
        {
            handles[i].sprite = chosenHandle;
        }
        for (int i = 0; i < pommels.Length; i++)
        {
            pommels[i].sprite = chosenPommel;
        }
    }

    #region Private Methods
    private void SetSpriteSheetName(string _sheetName)
    {
        spriteSheetName = _sheetName;
    }

    private void LoadSprites()
    {
        //Debug.Log("Called LoadSprites");
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

   private void LoadHandles()
    {
        //Debug.Log("Called LoadHandles");
        Sprite[] _sprites = Resources.LoadAll<Sprite>("_Final_Assets/" + folder + "/" + handleSheetName);
        //Debug.Log("Handle Sheet Name" + handleSheetName);
        for (int i = 0; i < _sprites.Length; i++)
        {
            //Debug.Log("Handle sprite sheet loaded: " + _sprites[i].name);
            string[] tempNames = _sprites[i].name.Split('_');
            if (tempNames[0] == "Handle")
            {
                _Handles.Add(_sprites[i]);
                //Debug.Log("Added a handle: " + _sprites[i].name);
            }
            else
            {
                _Pommels.Add(_sprites[i]);
                //Debug.Log("Added a pommel: " + _sprites[i].name);
            }
        }
        //Debug.Log("End of LoadHandles");
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

