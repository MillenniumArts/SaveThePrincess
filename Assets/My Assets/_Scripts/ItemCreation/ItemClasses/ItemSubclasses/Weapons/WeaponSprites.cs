using UnityEngine;
using System.Collections;

public class WeaponSprites : MonoBehaviour {
    public string[] weaponParts; // Set in the inspector.

    public Sprite[] currWeaponSprites;

    private SwapWeaponSprites _wSprites;

    public string tagName;

    void Start()
    {
        _wSprites = GameObject.FindObjectOfType<SwapWeaponSprites>();
        currWeaponSprites = new Sprite[10];
        RandomWeaponSprites();
        DisplaySprites();
        LoadSprites(SaveSprites());
    }
    
    public void RandomWeaponSprites()
    {
        currWeaponSprites = new Sprite[10];
        for (int i = 0; i < currWeaponSprites.Length; i++)
        {
            currWeaponSprites[i] = _wSprites.GetSprite(weaponParts[i]);
        }
    }

    public void DisplaySprites()
    {
		// For each Sprite Renderer in this objects children the search and replace is done.
		foreach (/*var*/ SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
		{
            if (renderer.gameObject.CompareTag(tagName))
            {
                for (int i = 0; i < currWeaponSprites.Length; i++)
                {	// For loop search.
                    Sprite newSprite = currWeaponSprites[i];		// Stores the current search item for comparison.
                    if(renderer.gameObject.name == weaponParts[i])
                    {				// If the sprite renderer's sprite name = the searched sprite name.
                        renderer.sprite = newSprite;				// The searched sprite is now the sprite renderer'd new sprite.
                        break;										// Break out of the for loop search
                    }
                }
            }
		}
	}

    public Sprite[] GetWeaponSprites()
    {
        return currWeaponSprites;
    }

    public void SwapWeaponSprites(WeaponSprites other)
    {
        Sprite[] tempSprites = new Sprite[other.GetWeaponSprites().Length];
        tempSprites = other.GetWeaponSprites();
        for (int i = 0; i < currWeaponSprites.Length; i++)
        {
            currWeaponSprites[i] = tempSprites[i];
        }
        DisplaySprites();
    }

    public string SaveSprites()
    {
        string s = null;
        for (int i = 0; i < currWeaponSprites.Length; i++)
        {
            string temp = currWeaponSprites[i].name;
            s += temp;
            s += "+";
        }
        //Debug.Log(s);
        //saved = s;
        return s;
    }

    //For testing

    /*public bool go = false;
    private string saved;
    void Update()
    {
        if (go)
        {
            LoadSprites(saved);
        }
    }*/

    public void LoadSprites(string savedSprites)
    {
        string[] spriteNames = savedSprites.Split('+');
        for (int i = 0; i < (spriteNames.Length - 1); i++)
        {
            //Debug.Log(spriteNames[i]);
            currWeaponSprites[i] = _wSprites.SearchForSprites(weaponParts[i], spriteNames[i]);
        }
        DisplaySprites();
    }
}
