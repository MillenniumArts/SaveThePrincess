using UnityEngine;
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
}
