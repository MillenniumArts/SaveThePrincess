using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Armor class.
/// </summary>
public class Armor : Item {
	/// <summary>
	/// The name of the class, Armor.
	/// </summary>
	[SerializeField] protected string className = "Armor";
	/// <summary>
	/// The animation parameter for the Armor class.
	/// </summary>
	[SerializeField] protected string animationParameter;
    /*
	/// <summary>
	/// The body armor type options.
	/// </summary>
	[SerializeField] protected string[] armorOptionsTypes;
	/// <summary>
	/// The body armor type options' sprites.
	/// </summary>
	[SerializeField] protected Sprite[] bodyArmorOptionsSprites;
	/// <summary>
	/// The helmet armor type options' sprites.
	/// </summary>
	[SerializeField] protected Sprite[] helmetArmorOptionsSprites;
	/// <summary>
	/// The back shoulder armor type options' sprites.
	/// </summary>
	[SerializeField] protected Sprite[] backShoulderArmorOptionsSprites;
	/// <summary>
	/// The front shoulder armor type options' sprites.
	/// </summary>
	[SerializeField] protected Sprite[] frontShoulderArmorOptionsSprites;
	/// <summary>
	/// The index of the armor type.  Used to match sets of srmor sprites.
	/// </summary>
	[SerializeField] protected int typeIndex;
     */
    [SerializeField]
    private string tagName, folder, spriteSheetName;
    [SerializeField]
    private int numberOfElements;
    [SerializeField]
    private int armourSetNum;
    /*
	/// <summary>
	/// Gets an index indicating the set of armor sprites.
	/// </summary>
	/// <returns>The type index.</returns>
	public int GetTypeIndex(){
		return typeIndex;
	}

	/// <summary>
	/// Copies the index of the armor type.
	/// </summary>
	/// <param name="a">The Armor to copy from.</param>
	public void CopyTypeIndex(Armor a){
		typeIndex = a.GetTypeIndex();
	}

	/// <summary>
	/// Gets the body sprite of the armor.
	/// </summary>
	/// <returns>Body armor sprite.</returns>
	public Sprite GetArmorSprite(){
		return bodyArmorOptionsSprites[typeIndex];
	}
    /// <summary>
    /// Gets the helmet sprite of the armor.
    /// </summary>
    /// <returns>Helmet sprite.</returns>
	public Sprite GetHelmetSprite(){
		return helmetArmorOptionsSprites[typeIndex];
	}
    /// <summary>
    /// Gets the front shoulder sprite of the armor.
    /// </summary>
    /// <returns>Front shoulder sprite.</returns>
	public Sprite GetFrontShoulderSprite(){
		return frontShoulderArmorOptionsSprites[typeIndex];
	}
    /// <summary>
    /// Gets the back shoulder sprite of the armor.
    /// </summary>
    /// <returns>Back shoulder sprite.</returns>
	public Sprite GetBackShoulderSprite(){
		return backShoulderArmorOptionsSprites[typeIndex];
	}

	/// <summary>
	/// Swaps the sprites from an instantiated armor to another armor's sprite array(at index 0).
    /// Currently used to move all the armor sprites to the sprite array of a ArmorBlank prefab.
	/// </summary>
	/// <param name="a">Armor</param>
	public void SwapArmorSprites(Armor a){
		bodyArmorOptionsSprites[0] = a.GetArmorSprite();
		helmetArmorOptionsSprites[0] = a.GetHelmetSprite();
		frontShoulderArmorOptionsSprites[0] = a.GetFrontShoulderSprite();
		backShoulderArmorOptionsSprites[0] = a.GetBackShoulderSprite();
	}
     * */

    #region SAVING
    public string SaveArmourSprites()
    {
        string saveString = spriteSheetName + "^" + armourSetNum;
        return saveString;
    }

    public void LoadArmourSprites(string _saveString)
    {
        string[] loadString = _saveString.Split('^');
        spriteSheetName = loadString[0];
        armourSetNum = int.Parse(loadString[1]);
        SendArmourSetToRenderer();
    }
    #endregion SAVING

    public string GetSpriteSheetName()
    {
        return spriteSheetName;
    }

    public int GetArmourSetNum()
    {
        return armourSetNum;
    }

    public void CopyOverSprites(Armor other)
    {
        spriteSheetName = other.GetSpriteSheetName();
        //numberOfElements = other.GetNumOfElements();
        armourSetNum = other.GetArmourSetNum();
        SendArmourSetToRenderer();
    }

    protected void RandomSetNum()
    {
        armourSetNum = Random.Range(0, numberOfElements);
    }

    protected void DisplayArmourInStore()
    {
        List<Sprite> tempList = new List<Sprite>();
        LoadSprites(armourSetNum, tempList);
    }

    private void LoadSprites(int _setNum, List<Sprite> _list)
    {
        Sprite[] _sprites = Resources.LoadAll<Sprite>(/*"_Final_Assets/" + */folder + "/" + spriteSheetName);
        for (int i = 0; i < _sprites.Length; i++)
        {
            string[] tempNames = _sprites[i].name.Split('_');
            if (tempNames[0] == _setNum.ToString())
            {
                _list.Add(_sprites[i]);
            }
        }
        RenderSprites(_list);
    }

    private void RenderSprites(List<Sprite> _sList)
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (renderer.gameObject.CompareTag(tagName))
            {
                for (int i = 0; i < _sList.Count; i++)
                {	// For loop search.
                    string[] spriteName = renderer.sprite.name.Split('_'); // Get the sprite name temporarily.
                    Sprite newSprite = _sList[i];		// Stores the current search item for comparison.
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

    protected void SendArmourSetToRenderer()
    {
        ArmourRenderer renderer = gameObject.GetComponentInParent<ArmourRenderer>();
        if(renderer != null)
            renderer.TransferSprites(spriteSheetName, armourSetNum);
    }
}
