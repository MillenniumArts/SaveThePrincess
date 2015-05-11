using UnityEngine;
using System.Collections;

/// <summary>
/// Does the heavy lifting for swapping sprites.
/// </summary>
public class SwapSprites : MonoBehaviour {

	/// <summary>
	/// This array will be filled with the combination that is passed to it.
	/// </summary>
	private Sprite[] customCombination;

	/// <summary>
	/// Loads the combination that is passed to it.
	/// </summary>
	/// <param name="folder">Folder that holds the sprites.</param>
	/// <param name="name">Name of the sprite sheet.</param>
	/// <param name="types">String array containing the types of the sprite sheet.</param>
	/// <param name="numberOfElements">Number of elements in the combination.</param>
	public void LoadCombination(string folder, string name, int[] types, int numberOfElements){
		customCombination = new Sprite[numberOfElements];
		for(int i = 0; i < numberOfElements; i++){
			/*var*/ Sprite[] subSprites = Resources.LoadAll<Sprite>(folder + "/" + name + types[i]);	// Sprites are loaded.
			customCombination[i] = subSprites[i];														// And set in to a temporary array.
		}	
		Swap(customCombination, numberOfElements);														// Swap function is called.
	}

	/// <summary>
	/// Swaps all the sprites that are currently displayed with new ones.
	/// </summary>
	/// <param name="spriteCombination">Sprite combination. An array of sprites that hold a combination to be displayed.</param>
	void Swap(Sprite[] spriteCombination, int num){
		
		// For each Sprite Renderer in this objects children the search and replace is done.
		foreach (/*var*/ SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
		{
			string spriteName = renderer.sprite.name;			// Get the sprite name temporarily.
			for(int i = 0; i < spriteCombination.Length; i++){	// For loop search.
				Sprite newSprite = spriteCombination[i];		// Stores the current search item for comparison.
				if(spriteName == newSprite.name){				// If the sprite renderer's sprite name = the searched sprite name.
					renderer.sprite = newSprite;				// The searched sprite is now the sprite renderer'd new sprite.
					break;										// Break out of the for loop search
				}
			}
		}
	}

}