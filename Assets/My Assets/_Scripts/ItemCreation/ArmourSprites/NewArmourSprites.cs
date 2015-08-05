using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NewArmourSprites : MonoBehaviour {
    [SerializeField]
    private string folder, spriteSheetName;
    [SerializeField]
    private Sprite chosenArmour, chosenHelmet;
    private List<Sprite> _Armour, _Helmet;

    void Start()
    {
        _Armour = new List<Sprite>();
        _Helmet = new List<Sprite>();
    }

    public void GenerateNewSprites()
    {
        LoadSprites();
        int rand1 = RandomNumberGenerator(_Armour.Count);
        int rand2 = RandomNumberGenerator(_Helmet.Count);
        SetChosenSprites(rand1, rand2);
        _Armour.Clear();
        _Helmet.Clear();
        _Armour = new List<Sprite>();
        _Helmet = new List<Sprite>();
    }

    public string GetSpriteSheetName()
    {
        return spriteSheetName;
    }

    public Sprite GetArmourSprite()
    {
        return chosenArmour;
    }

    public Sprite GetHelmetSprite()
    {
        return chosenHelmet;
    }

    public void SetSpriteSheetName(string _sheetName)
    {
        spriteSheetName = _sheetName;
    }

    public void SetArmourSprite(Sprite armour)
    {
        chosenArmour = armour;
    }

    public void SetHelmetSprite(Sprite helmet)
    {
        chosenHelmet = helmet;
    }

    private void LoadSprites()
    {
        Sprite[] _sprites = Resources.LoadAll<Sprite>("_Final_Assets/" + folder + "/" + spriteSheetName);
        for (int i = 0; i < _sprites.Length; i++)
        {
            string[] tempNames = _sprites[i].name.Split('_');
            if (tempNames[0] == "Helmet")
            {
                _Helmet.Add(_sprites[i]);
            }
            else
            {
                _Armour.Add(_sprites[i]);
            }
        }
    }

    private int RandomNumberGenerator(int MAX)
    {
        int r = Random.Range(0, MAX);
        return r;
    }

    private void SetChosenSprites(int armourIndex, int helmetIndex)
    {
        chosenArmour = _Armour[armourIndex];
        chosenHelmet = _Helmet[helmetIndex];
    }
}
