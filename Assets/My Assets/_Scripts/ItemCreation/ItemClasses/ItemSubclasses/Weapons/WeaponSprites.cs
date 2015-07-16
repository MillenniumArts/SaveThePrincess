using UnityEngine;
using System.Collections;

public class WeaponSprites : MonoBehaviour {
    public string[] weaponParts;

    public Sprite[] currWeaponSprites;

    private SwapWeaponSprites _wSprites;

    public string tagName;

    void Start()
    {
        _wSprites = GameObject.FindObjectOfType<SwapWeaponSprites>();
        currWeaponSprites = new Sprite[10];
        DisplaySprites();
        RandomWeaponSprites();
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
}
